using BenchmarkDotNet.Running;
using ResultVsException.Benchmarks;

BenchmarkRunner.Run<ExceptionVsOperationResultBenchmark>();
