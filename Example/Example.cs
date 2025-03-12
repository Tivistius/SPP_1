using System;
using Tracer.Core;
using Tracer.Serialization;
using System.IO;
using System.Collections.Generic;
using Tracer.Serialization.Abstractions;
using System.Threading;

namespace Tracer.Example
{
    class Example
    {
        private static ITracer _tracer = new Core.Tracer();
        static void Main()
        {
            var pluginLoader = new PluginLoader(".");
            List<ITraceResultSerializer> serializers = pluginLoader.LoadPlugins();

            var thread1 = new Thread(MethodX);
            var thread2 = new Thread(MethodX);
            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();
            foreach (ITraceResultSerializer serializer in serializers)
            {
                serializer.Serialize(_tracer.GetTraceResult(), File.Create("res." + serializer.Format));
            }
        }
        static void MethodX()
        {
            _tracer.StartTrace();
            MethodA();
            _tracer.StopTrace();
            _tracer.StartTrace();
            MethodA();
            _tracer.StopTrace();
        }
        static void MethodA()
        {
            int y = 0;
            int x = 100000000;
            for (int i = 0; i < x; i++)
            {
                y += x + 1;
            }
            _tracer.StartTrace();
            MethodB();
            _tracer.StopTrace();
        }

        static void MethodB()
        {
            int y = 0;
            int x = 10000000;
            for(int i = 0; i < x; i++)
            {
                y += x + 1;
            }
            MethodC();
        }

        static void MethodC()
        {
            return;
        }
    }
}