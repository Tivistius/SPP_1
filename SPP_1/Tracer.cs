using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
namespace Tracer.Core
{
    public interface ITracer
    {
        void StartTrace();
        void StopTrace();
        IReadOnlyTraceResult GetTraceResult();
    }
    public class Tracer : ITracer
    {
        private class TreadState
        {
            public  Stack<Stopwatch> stopwatchStack = new Stack<Stopwatch>();
            public  Stack<TraceResult.MethodResults> resultStack = new Stack<TraceResult.MethodResults>();
            public  List<TraceResult.MethodResults> rootResults = new List<TraceResult.MethodResults>();
        }
        private Dictionary<int, TreadState> _treadResults = new Dictionary<int, TreadState>();
        public void StartTrace()
        {
            var stackTrace = new StackTrace();
            var frame = stackTrace.GetFrame(1);
            var method = frame.GetMethod();
            var methodName = method.Name;
            var className = method.DeclaringType.FullName;

            var traceResult = new TraceResult.MethodResults
            {
                MethodName = methodName,
                ClassName = className
            };
            int threadId = Thread.CurrentThread.ManagedThreadId;
            if (!_treadResults.ContainsKey(threadId))
            {
                _treadResults.Add(threadId, new TreadState());
            }
            if (_treadResults[threadId].resultStack.Count > 0)
            {
                _treadResults[threadId].resultStack.Peek().InnerMethods.Add(traceResult);
            }
            else
            {
                _treadResults[threadId].rootResults.Add(traceResult);
            }

            _treadResults[threadId].resultStack.Push(traceResult);

            var stopwatch = new Stopwatch();
            _treadResults[threadId].stopwatchStack.Push(stopwatch);
            stopwatch.Start();
        }

        public void StopTrace()
        {
            int threadId = Thread.CurrentThread.ManagedThreadId;
            if (_treadResults[threadId].stopwatchStack.Count == 0 || _treadResults[threadId].resultStack.Count == 0)
            {
                throw new InvalidOperationException("No active trace to stop.");
            }

            var stopwatch = _treadResults[threadId].stopwatchStack.Pop();
            stopwatch.Stop();

            var traceResult = _treadResults[threadId].resultStack.Pop();
            traceResult.ExecutionTime = stopwatch.ElapsedMilliseconds;
        }
        private long GetTotalExecutionTime(List<TraceResult.MethodResults> methods)
        {
            long res = 0;
            foreach(var method in methods)
            {
                res += method.ExecutionTime;
            }
            return res;
        }
        public IReadOnlyTraceResult GetTraceResult()
        {
            TraceResult res = new TraceResult();
            int i = 1;
            foreach(var tread in _treadResults)
            {
                res.Threads.Add(new TraceResult.TreadResult
                {
                    Id = i++,
                    Time = GetTotalExecutionTime(tread.Value.rootResults),
                    Methods = tread.Value.rootResults
                });
            }
            return res;
        }
    }
}
