# Result Pattern vs Exception — Benchmark

Benchmark comparing two error-handling strategies in .NET:

- **Exception**: throw/catch using `ArgumentOutOfRangeException`
- **Result pattern**: return a `Result<T>` via [Divino.OperationResult](https://github.com/victorDivino/operationResult)

## Motivation

Exceptions in .NET are expensive on the failure path — the runtime must capture a stack trace, unwind the call stack, and allocate the exception object. The Result pattern avoids all of that by returning a value that carries either a success or a failure, with zero allocations on either path.

This project measures exactly how much that difference matters in practice.

## Benchmarks

Each benchmark simulates a single domain operation (`ParseAge`) as it would appear in real production code. BenchmarkDotNet runs it in a tight loop internally to produce stable measurements.

| Category | Benchmark          | What it measures                         |
|----------|--------------------|------------------------------------------|
| Success  | Exception \| success | try/catch with no exception thrown      |
| Success  | Result \| success    | Result check on a successful operation  |
| Failure  | Exception \| failure | try/catch when an exception is thrown   |
| Failure  | Result \| failure    | Result check on a failed operation      |

The **Success** category shows the overhead of the try/catch block alone (no exception thrown).
The **Failure** category is where the real cost difference lives — throwing vs returning a Result.

## Stack

- .NET 10
- [BenchmarkDotNet](https://benchmarkdotnet.org/) 0.14.0
- [Divino.OperationResult](https://github.com/victorDivino/operationResult) 4.0.1

## Running locally

```bash
dotnet run --project ResultVsException.Benchmarks -c Release
```

> Results must be run in **Release** mode. BenchmarkDotNet will warn if you run in Debug.
