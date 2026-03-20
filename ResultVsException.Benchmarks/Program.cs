using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using ResultVsException.Benchmarks;

// Run all three benchmark classes in sequence.
// Each one answers a different question about the Exception vs OperationResult trade-off.
BenchmarkRunner.Run(
[
    BenchmarkConverter.TypeToBenchmarks(typeof(SingleCallBenchmark)),
    BenchmarkConverter.TypeToBenchmarks(typeof(ApiBatchBenchmark)),
    BenchmarkConverter.TypeToBenchmarks(typeof(GcPressureBenchmark)),
],
    DefaultConfig.Instance
);
