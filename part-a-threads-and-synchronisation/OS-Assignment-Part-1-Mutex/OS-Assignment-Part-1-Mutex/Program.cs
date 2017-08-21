using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OS_Assignment_Part_1_Mutex {
    using System;
    using System.Threading;

    class Example {
        // Create a new Mutex. The creating thread does not own the mutex.
        private static Mutex mut = new Mutex();
        private static Mutex _rotatorMutex = new Mutex();
        private const int numIterations = 1;
        private const int numThreads = 3;

        static void Main(string[] args) {
            if (args.Length > 0)
                Console.WriteLine(args[0]);
            var loaderThread = new Thread(Loader) { Name = "Loader" };
            var rotatorThread = new Thread(Rotator) { Name = "Rotator" };
            var pickerThread = new Thread(Picker) { Name = "Picker" };
            loaderThread.Start();
            Thread.Sleep(100);
            rotatorThread.Start();
            pickerThread.Start();
            // The main thread exits, but the application continues to
            // run until all foreground threads have exited.
            Console.Read();
        }

        private static void Picker() {
            while (true) {
                Console.WriteLine("Picker() : Waiting for rotator . . .");
                _rotatorMutex.WaitOne();
                Console.WriteLine("Picker() : Picking item . . .");
                Thread.Sleep(500);
                _rotatorMutex.ReleaseMutex();
            }
        }

        private static void Rotator() {
            while (true) {
                Console.WriteLine("Rotator() : Waiting for loader and picker . . .");
                _rotatorMutex.WaitOne();
                Console.WriteLine("Rotator() : Rotating . . . ");
                Thread.Sleep(500);
                _rotatorMutex.ReleaseMutex();
            }
        }

        private static void Loader() {
            while (true) {
                Console.WriteLine("Loader() : Waiting for rotator . . .");
                _rotatorMutex.WaitOne();
                Console.WriteLine("Loader() : Loading item . . .");
                Thread.Sleep(500);
                _rotatorMutex.ReleaseMutex();
            }

        }

        private static void ThreadProc() {
            for (int i = 0 ; i < numIterations ; i++) {
                UseResource();
            }
        }

        // This method represents a resource that must be synchronized
        // so that only one thread at a time can enter.
        private static void UseResource() {
            // Wait until it is safe to enter.
            Console.WriteLine("{0} is requesting the mutex" ,
                Thread.CurrentThread.Name);
            mut.WaitOne();


            Console.WriteLine("{0} has entered the protected area" ,
                Thread.CurrentThread.Name);

            // Place code to access non-reentrant resources here.

            // Simulate some work.
            Thread.Sleep(500);

            Console.WriteLine("{0} is leaving the protected area" ,
                Thread.CurrentThread.Name);

            // Release the Mutex.
            mut.ReleaseMutex();
            Console.WriteLine("{0} has released the mutex" ,
                Thread.CurrentThread.Name);
        }
    }
}
