using System.Collections.Generic;
namespace Tracer.Core
{
    public interface IReadOnlyMethodResults
    {
        string MethodName { get; }
        string ClassName { get; }
        long ExecutionTime { get; }
        IReadOnlyList<IReadOnlyMethodResults> InnerMethods { get; }
    }

    public interface IReadOnlyThreadResult
    {
        int Id { get; }
        long Time { get; }
        IReadOnlyList<IReadOnlyMethodResults> Methods { get; }
    }

    public interface IReadOnlyTraceResult
    {
        IReadOnlyList<IReadOnlyThreadResult> Threads { get; }
    }
    public class TraceResult : IReadOnlyTraceResult
    {
        public class MethodResults : IReadOnlyMethodResults
        {
            public string MethodName { get; set; }
            public string ClassName { get; set; }
            public long ExecutionTime { get; set; }
            public List<MethodResults> InnerMethods { get; set; } = new List<MethodResults>();
            IReadOnlyList<IReadOnlyMethodResults> IReadOnlyMethodResults.InnerMethods => InnerMethods.AsReadOnly();
        }
        public class TreadResult : IReadOnlyThreadResult
        {
            public int Id;
            public long Time;
            public List<MethodResults> Methods  { get; set; } = new List<MethodResults>();
            int IReadOnlyThreadResult.Id => Id;
            long IReadOnlyThreadResult.Time => Time;
            IReadOnlyList<IReadOnlyMethodResults> IReadOnlyThreadResult.Methods => Methods.AsReadOnly();
        }
        public List<TreadResult> Threads { get; set; } = new List<TreadResult>();

        IReadOnlyList<IReadOnlyThreadResult> IReadOnlyTraceResult.Threads => Threads.AsReadOnly();
    }
}
