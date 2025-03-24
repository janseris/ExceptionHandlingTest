namespace ConsoleApp1
{
    //How to use: run without debugger attached and uncomment relevant sections you want to test
    internal class Program
    {
        private static event EventHandler Event1;
        private static event EventHandler Event2;
        private static event EventHandler Event3;

        static void Main(string[] args)
        {
            Event1 += ThrowAwaitTaskRunException;
            Event2 += ThrowTaskRunExceptionNoAwait;
            Event3 += ThrowException;

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            //Console.WriteLine("Press any key to throw Task Run exception (no await) in event handler");
            //Console.ReadKey();
            //Event2.Invoke(null, EventArgs.Empty); //no exception handler is invoked and the proces does not terminate

            //Console.WriteLine("Press any key to throw exception in event handler");
            //Console.ReadKey();
            //Event3.Invoke(null, EventArgs.Empty); //process terminates in CurrentDomain_UnhandledException

            //Console.WriteLine("Press any key to throw await Task Run exception in event handler");
            //Console.ReadKey();
            //Event1.Invoke(null, EventArgs.Empty); //process terminates in CurrentDomain_UnhandledException 

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        //Console app process terminates
        private static void ThrowException(object sender, EventArgs e)
        {
            Console.WriteLine($"Throwing exception from event handler");
            throw new Exception(); //triggers UnhandledException
        }

        //Console app process terminates
        private static async void ThrowAwaitTaskRunException(object sender, EventArgs e)
        {
            await Task.Run(() => 
            {
                Console.WriteLine($"Throwing exception from event handler using await Task.Run()");
                throw new Exception();
            }
            ); //triggers UnhandledException
        }

        //Console app process does not terminate
        private static void ThrowTaskRunExceptionNoAwait(object sender, EventArgs e)
        {
            Task.Run(() => 
            {
                Console.WriteLine($"Throwing exception from event handler using Task.Run() without await");
                throw new Exception(); //the exception is not reported. anywhere
            });
        }

        //This seemingly never happens even when Garbage Collector is called
        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            Console.WriteLine($"Unobserved task exception");
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine($"Unhandled exception, CLR is terminating: {e.IsTerminating}");
        }
    }
}
