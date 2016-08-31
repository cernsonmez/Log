using Ninject;
using System;
using System.Reflection;

namespace Log.DataGeneration
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Load(Assembly.GetExecutingAssembly());

            var logic = new ServiceLogic(kernel.Get<IConsumerConnection<LogInfo>>(), kernel.Get<IServiceConnection<LogInfo>>(), kernel.Get<IDatabaseConnection>());
            logic.InitiateService();

            Console.ReadKey();
        }
    }
}
