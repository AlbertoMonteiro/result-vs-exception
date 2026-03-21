using BenchmarkDotNet.Running;
using ResultVsException.Benchmarks;

BenchmarkRunner.Run<SingleCallBenchmark>();
BenchmarkRunner.Run<ApiBatchBenchmark>();
BenchmarkRunner.Run<GcPressureBenchmark>();
