

using System;

namespace UserManagement.Services.Results;

public interface IOperationResult
{
    public Boolean IsSuccess { get; }
}

public interface IOperationResult<T> : IOperationResult
{
    public T? Result { get; }
}

public record SuccessResult : IOperationResult
{
    Boolean IOperationResult.IsSuccess => true;
}

public record SuccessResult<T> : SuccessResult, IOperationResult<T>
{
    public T Result { get; }

    public SuccessResult(T result)
    {
        Result = result;
    }
}

public record ErrorResult : IOperationResult
{
    Boolean IOperationResult.IsSuccess => false;

    public String ErrorMessage { get; }
    public Exception? Exception { get; }

    public ErrorResult(String errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public ErrorResult(Exception error, String? errorMessage = null)
    {
        Exception = error;
        ErrorMessage = errorMessage ?? error.Message;
    }
}

public record ErrorResult<T> : ErrorResult, IOperationResult<T>
{
    public T? Result => default;
    public ErrorResult(String errorMessage)
        : base(errorMessage) { }

    public ErrorResult(Exception error, String? errorMessage = null)
        : base(error, errorMessage) { }
}
