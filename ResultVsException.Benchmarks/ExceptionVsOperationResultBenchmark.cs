using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using Divino.OperationResult;

namespace ResultVsException.Benchmarks;

[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ExceptionVsOperationResultBenchmark
{
    private const string ErrorMessage = "Validation failed: value is out of range.";

    // -------------------------------------------------------------------------
    // Exception-based approach
    // -------------------------------------------------------------------------

    /// <summary>
    /// Success path: exception is never thrown.
    /// </summary>
    [Benchmark(Description = "Exception – success path")]
    public int ExceptionSuccess()
    {
        try
        {
            return ComputeWithException(valid: true);
        }
        catch (InvalidOperationException)
        {
            return -1;
        }
    }

    /// <summary>
    /// Failure path: exception IS thrown and caught every iteration.
    /// </summary>
    [Benchmark(Description = "Exception – failure path")]
    public int ExceptionFailure()
    {
        try
        {
            return ComputeWithException(valid: false);
        }
        catch (InvalidOperationException)
        {
            return -1;
        }
    }

    // -------------------------------------------------------------------------
    // OperationResult-based approach
    // -------------------------------------------------------------------------

    /// <summary>
    /// Success path: result carries a value, no exception is involved.
    /// </summary>
    [Benchmark(Description = "OperationResult – success path")]
    public int OperationResultSuccess()
    {
        var result = ComputeWithResult(valid: true);
        return result.IsSuccess ? result.Value : -1;
    }

    /// <summary>
    /// Failure path: result carries an exception without stack-unwinding.
    /// </summary>
    [Benchmark(Description = "OperationResult – failure path")]
    public int OperationResultFailure()
    {
        var result = ComputeWithResult(valid: false);
        return result.IsSuccess ? result.Value : -1;
    }

    // -------------------------------------------------------------------------
    // Helper methods
    // -------------------------------------------------------------------------

    private static int ComputeWithException(bool valid)
    {
        if (!valid)
            throw new InvalidOperationException(ErrorMessage);

        return 42;
    }

    private static Result<int> ComputeWithResult(bool valid)
    {
        if (!valid)
            return new InvalidOperationException(ErrorMessage);

        return 42;
    }
}
