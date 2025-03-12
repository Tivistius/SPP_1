using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;

namespace Tracer.Core.Test
{
    [TestClass]
    public class TracerTests
    {
        [TestMethod]
        public void BaseWorkingTest()
        {
            ITracer tracer = new Tracer();
            tracer.StartTrace();
            MethodA(tracer);
            tracer.StopTrace();

        }
        [TestMethod]
        public void NoStartTraceTest()
        {
            ITracer tracer = new Tracer();
            MethodA(tracer);
            try
            {
                tracer.StopTrace();
            }
            catch (InvalidOperationException e)
            {
                StringAssert.Contains(e.Message, "No active trace to stop.");
                return;
            }
            Assert.Fail("The expected exception was not thrown.");
        }
        [TestMethod]
        public void MultiTreadingTest()
        {
            ITracer tracer = new Tracer();
            var thread1 = new Thread(MethodX);
            var thread2 = new Thread(MethodX);
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
        }
        [TestMethod]
        public void NoFunctionCallTest()
        {
            ITracer tracer = new Tracer();
            tracer.StartTrace();
            tracer.StopTrace();
        }
        static void MethodX(object data)
        {
            ITracer tracer = (ITracer)data;
            tracer.StartTrace();
            MethodA(tracer);
            tracer.StopTrace();
            tracer.StartTrace();
            MethodA(tracer);
            tracer.StopTrace();
        }
        static void MethodA(ITracer tracer)
        {
            int y = 0;
            int x = 100000000;
            for (int i = 0; i < x; i++)
            {
                y += x + 1;
            }
            tracer.StartTrace();
            MethodB();
            tracer.StopTrace();
        }
        static void MethodB()
        {
            int y = 0;
            int x = 10000000;
            for (int i = 0; i < x; i++)
            {
                y += x + 1;
            }
        }
    }
}
