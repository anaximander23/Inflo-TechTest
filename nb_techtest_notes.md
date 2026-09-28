# Tech Test Notes

## Design & Implementation Details

### Aliases cs CLR types
An uncommon practice among C# developers, but I prefer to use type names over CLR aliases (eg. `Int32` vs. `int`, `String` vs. `string`) - it makes for consistent syntax highlighting and reduces confusion and cognitive overhead (for example, it's easier/more intuitive to remember that `Convert.ToInt64(...)` returns an `Int64`, rather than `long`). It's a small thing, but I've come to prefer it.

### IOperationResult
A simple option type to allow methods to report failures without exceptions. Inspired by functional programming patterns.
I've built this a few ways over the years; this is one example but there are other shapes (this is perhaps to the more formalised/structured end of the spectrum). C# 15's new discriminated union types might be an interesting way to build a "result" option type, but I haven't tried it yet.

Exceptions...
* involve a hard stop and stack unwind, which comes with significant performance impact
* walk up the stack to find the nearest applicable catch block, which is essentially a goto - an engineer reading the `throw` has no way to know where it'll be caught
* are noisy; many frameworks automatically log exceptions (to the console, to logfiles, to telemetry and monitoring systems). Using exceptions for regular operations generates noise that risks obscuring genuine problems or desensitising engineers to error signals.

The use of a "result" option type provides a sidechannel through which to report rich error information without exceptions, leaving the throwing of exceptions for scenarios that were not anticipated by the developer or are known to be unrecoverable.
This also allows for simpler code with less exception handling boilerplate, because most scenarios that will throw exceptions are those in which something has gone wrong, and loudly terminating the operation (or the application) is probably an appropriate response -
it attracts attention, and starting over is often the fastest and surest way back to a known-good state.

### On Async/Await
There is no async/await used around the data access. I would probably async all of the things.
In the ASP.NET Core end of things, it allows the worker thread to go back and accept another incoming request while the database works, which can prevent one heavy database call from slowing down lighter/faster endpoints.
It also allows us to tie underlying operations to the HttpContext cancellation token, so we can cancel heavy work if the connection is closed (eg. the user navigates away, closes their browser, etc).
Deeper into the application, it allows domain logic to do more work with fewer threads, and to run heavy database operations in parallel where possible.

### The Data Access Layer
The `IDataContext` interface is likely more of an impediment than a benefit. DbContext can be mocked for testing without needing the interface (and/or can be configured to use an in-memory database for testing), so the interface doesn't add anything, but it does hide useful EF features from the user service where they could help in querying (eg. access to the change tracker, controls like `.AsNoTracking()`, etc.). EF can be pointed at various different backing stores, and moving to something EF doesn't support would likely mean rewriting a chunk of the user service anyway - there's only so much you can hide the fact that your data lives in SQL before you lose the ability to make use of the features that made SQL a good choice, and likewise, trying to make the user service fit any given store equally means it fails to take advantage of the features of any of them.

I'd probably remove the `IDataContext` interface and use `DbContext` directly. EF+LINQ is already enough of an abstraction over the mechanics of querying the database for most scenarios.
