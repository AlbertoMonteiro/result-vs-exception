namespace ResultVsException.Benchmarks;

/// <summary>
/// Minimal Result type — carries either a value (success) or an exception (failure)
/// without throwing. Implicit conversions mirror the Divino.OperationResult API
/// used in the original project.
/// </summary>
public readonly struct Result<T>
{
    private readonly T? _value;
    private readonly Exception? _exception;

    private Result(T value)
    {
        _value = value;
        _exception = null;
        IsSuccess = true;
    }

    private Result(Exception exception)
    {
        _value = default;
        _exception = exception;
        IsSuccess = false;
    }

    public bool IsSuccess { get; }
    public T Value => _value!;
    public Exception? Exception => _exception;

    public static implicit operator Result<T>(T value) => new(value);
    public static implicit operator Result<T>(Exception exception) => new(exception);
}
