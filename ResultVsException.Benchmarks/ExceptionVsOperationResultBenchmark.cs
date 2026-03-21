using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Order;
using OperationResult;

namespace ResultVsException.Benchmarks;

/// <summary>
/// Compares throwing an exception vs returning an OperationResult.
///
/// Each benchmark represents a single operation — exactly as it would appear
/// in production code. BenchmarkDotNet runs it in a tight loop internally to
/// get stable measurements; the [Benchmark] method itself does one throw / one return.
///
/// Success path: no error — both approaches should be identical.
/// Failure path: error occurs — this is where the cost difference lives.
/// </summary>
[MemoryDiagnoser]
[Orderer(SummaryOrderPolicy.FastestToSlowest)]
[RankColumn]
public class ExceptionVsOperationResultBenchmark
{
    private const string ErrorMessage = "Validation failed: value is out of range.";

    // -------------------------------------------------------------------------
    // Success path — no error, no exception
    // -------------------------------------------------------------------------

    [Benchmark(Description = "Exception  | success")]
    public int Exception_Success()
    {
        try { return ParseAge(25); }
        catch { return -1; }
    }

    [Benchmark(Description = "Result     | success")]
    public int Result_Success()
    {
        var r = ParseAgeResult(25);
        return r.IsSuccess ? r.Value : -1;
    }

    // -------------------------------------------------------------------------
    // Failure path — one throw vs one Result carrying the exception
    // -------------------------------------------------------------------------

    [Benchmark(Description = "Exception  | failure")]
    public int Exception_Failure()
    {
        try { return ParseAge(-1); }
        catch { return -1; }
    }

    [Benchmark(Description = "Result     | failure", Baseline = true)]
    public int Result_Failure()
    {
        var r = ParseAgeResult(-1);
        return r.IsSuccess ? r.Value : -1;
    }

    // -------------------------------------------------------------------------
    // Domain methods — one throw / one Result per call, just like real code
    // -------------------------------------------------------------------------

    private static int ParseAge(int age)
    {
        if (age < 0 || age > 150)
            throw new ArgumentOutOfRangeException(nameof(age), ErrorMessage);
        return age;
    }

    private static Result<int> ParseAgeResult(int age)
    {
        if (age < 0 || age > 150)
            return new ArgumentOutOfRangeException(nameof(age), ErrorMessage);
        return age;
    }
}
