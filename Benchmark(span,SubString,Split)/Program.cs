using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;

namespace Benchmark_span_SubString_Split_;

internal class Program
{

    private class CustomConfig : ManualConfig
    {
        public CustomConfig()
        {
            AddJob(new Job());
            AddValidator(JitOptimizationsValidator.DontFailOnError);
            AddLogger(ConsoleLogger.Default);
            AddColumnProvider(DefaultColumnProviders.Instance);
        }
    }

    static void Main(string[] args)
    {

        var _date = new SpanPerformanceBenchmark();
        Console.WriteLine(_date.WithSplit());
        Console.WriteLine(_date.WithSubString());
        Console.WriteLine(_date.WithSpan());
      

         BenchmarkRunner.Run<SpanPerformanceBenchmark>(new CustomConfig());
    }
}

[MemoryDiagnoser]
public class SpanPerformanceBenchmark
{
    private readonly string _date = "2023-11-27-20-32";

    [Benchmark]
    public DateTime WithSubString()
    {
        var year = _date.Substring(0, 4);
        var month =_date.Substring(5, 2);
        var day =_date.Substring(8, 2);
        var hour =_date.Substring(11, 2);
        var minutes =_date.Substring(14, 2);

        var result = new DateTime(int.Parse(year),int.Parse(month),int.Parse(day)
            ,int.Parse(hour),int.Parse(minutes),1);
        return result;
    }

    [Benchmark]
    public DateTime WithSplit()
    {
        var SplitedDate = _date.Split('-');
        var year = SplitedDate[0];
        var month = SplitedDate[1];
        var day = SplitedDate[2];
        var hour = SplitedDate[3];
        var minutes = SplitedDate[4];

        var result = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day)
            , int.Parse(hour), int.Parse(minutes), 1);
        return result;
    }

    [Benchmark]
    public DateTime WithSpan()
    {
        ReadOnlySpan<char> dateSpan = _date;

        var year = dateSpan.Slice(0, 4);
        var month = dateSpan.Slice(5, 2);
        var day = dateSpan.Slice(8, 2);
        var hour = dateSpan.Slice(11, 2);
        var minutes = dateSpan.Slice(14, 2);

        var result = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day)
            , int.Parse(hour), int.Parse(minutes), 1);
        return result;
    }
}