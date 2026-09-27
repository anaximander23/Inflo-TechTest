# Tech Test Notes

## Design & Implementation Details

### IOperationResult
Simple option type to allow methods to report failures without exceptions. Inspired by functional programming patterns.
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

