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
        private static readonly Mutex RotatorMutex = new Mutex();
        private static readonly ManualResetEvent LoadedEvent = new ManualResetEvent(false);
        private static readonly ManualResetEvent PickedEvent = new ManualResetEvent(true);
        private static readonly ManualResetEvent LoadPlaceEmpty = new ManualResetEvent(true);
        private static readonly ManualResetEvent PickPlaceFull = new ManualResetEvent(false);
        private static readonly ManualResetEvent AllItemsDelivered = new ManualResetEvent(false);

        private static int _numberOfItemsToBeDelivered;
        private static int _numberOfItemsLoaded;
        private static int _numberOfItemsPicked;
        static int Main(string[] args) {
            if (!ValidateUserInput(args)) return -1;
            var loaderThread = new Thread(Loader) { Name = "Loader" };
            var rotatorThread = new Thread(Rotator) { Name = "Rotator" };
            var pickerThread = new Thread(Picker) { Name = "Picker" };
            loaderThread.Start();
            rotatorThread.Start();
            pickerThread.Start();
            AllItemsDelivered.WaitOne();
            Console.WriteLine("========================================");
            Console.WriteLine("Main():\tTask completed.");
            Console.WriteLine($"Main():\tSuccessfully delivered {_numberOfItemsToBeDelivered} items.");
            Console.WriteLine("Main():\tAborting Loader() . . .");
            loaderThread.Abort();
            Console.WriteLine("Main():\tAborting Rotator() . . .");
            rotatorThread.Abort();
            Console.WriteLine("Main():\tAborting Picker() . . .");
            pickerThread.Abort();
            Console.WriteLine("Main():\tAbortions completed.");
            return 0;
        }

        private static bool ValidateUserInput(string[] args) {
            if (args.Length == 1) {
                if (!int.TryParse(args[0] , out _numberOfItemsToBeDelivered)) {
                    Console.WriteLine($"Error : '{args[0]}' is not an integer.");
                    return false;
                }
                if (_numberOfItemsToBeDelivered < 1) {
                    Console.WriteLine("Argument must be more than 1.");
                    return false;
                }
                Console.WriteLine("Number of items to be delivered is set to " + _numberOfItemsToBeDelivered);
                return true;
            }
            Console.WriteLine("Error : Please pass in an integer argument that specify the number of items to be delivered.");
            return false;
        }

        private static void Picker() {
            while (true) {
                Console.WriteLine("Picker():\tWaiting for rotator . . .");
                PickPlaceFull.WaitOne();
                RotatorMutex.WaitOne();
                Console.WriteLine("Picker():\tRotator is free now.");
                Console.WriteLine($"Picker():\tPicking item #{_numberOfItemsPicked} . . .");
                _numberOfItemsPicked++;
                if (_numberOfItemsLoaded == _numberOfItemsToBeDelivered
                    &&
                    _numberOfItemsPicked == _numberOfItemsToBeDelivered)
                    AllItemsDelivered.Set();
                PickPlaceFull.Reset();
                PickedEvent.Set();
                RotatorMutex.ReleaseMutex();
            }
        }

        private static void Rotator() {
            while (true) {
                Console.WriteLine("Rotator():\tWaiting for loader and picker . . .");
                LoadedEvent.WaitOne();
                PickedEvent.WaitOne();
                RotatorMutex.WaitOne();
                if (_numberOfItemsPicked == _numberOfItemsToBeDelivered) return;
                Console.WriteLine("Rotator():\tItem is loaded on the left.");
                if (_numberOfItemsPicked > 0) Console.WriteLine("Rotator():\tItem is picked on the right.");
                Console.WriteLine("========================================");
                Console.WriteLine("Rotator():\tRotating . . . ");
                Console.WriteLine("Rotation completed.");
                Console.WriteLine("========================================");
                PickedEvent.Reset();
                LoadedEvent.Reset();
                PickPlaceFull.Set();
                LoadPlaceEmpty.Set();
                RotatorMutex.ReleaseMutex();
            }
        }

        private static void Loader() {
            while (true) {
                Console.WriteLine("Loader():\tWaiting for rotator . . .");
                LoadPlaceEmpty.WaitOne();
                RotatorMutex.WaitOne();
                if (_numberOfItemsLoaded == _numberOfItemsToBeDelivered) return;
                Console.WriteLine("Loader():\tRotator is free now.");
                Console.WriteLine($"Loader():\tLoading item #{_numberOfItemsLoaded} . . .");
                _numberOfItemsLoaded++;
                LoadPlaceEmpty.Reset();
                LoadedEvent.Set();
                RotatorMutex.ReleaseMutex();
            }

        }
    }
}
