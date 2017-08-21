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
        private static readonly Mutex RotatorMutex = new Mutex();
        private static bool _loaded = false;
        private static bool _picked = true;
        private static int _numberOfItemsToBeDelivered;
        private static int _numberOfItemsLoaded;
        private static int _numberOfItemsPicked;
        static int Main(string[] args) {
            if (args.Length == 1) {
                if (!int.TryParse(args[0] , out _numberOfItemsToBeDelivered)) {
                    Console.WriteLine($"Error : '{args[0]}' is not an integer.");
                    return -1;
                }
                else {
                    Console.WriteLine("Number of items to be delivered is set to " + _numberOfItemsToBeDelivered);
                }
            }
            else {
                Console.WriteLine("Error : Please pass in an integer argument that specify the number of items to be delivered.");
                return -1;
            }
            var loaderThread = new Thread(Loader) { Name = "Loader" };
            var rotatorThread = new Thread(Rotator) { Name = "Rotator" };
            var pickerThread = new Thread(Picker) { Name = "Picker" };
            loaderThread.Start();
            Thread.Sleep(100);
            rotatorThread.Start();
            pickerThread.Start();
            while (true) {
                if (_numberOfItemsLoaded == _numberOfItemsToBeDelivered
                    &&
                    _numberOfItemsPicked == _numberOfItemsToBeDelivered) {
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
            }
        }

        private static void Picker() {
            while (true) {
                if (_numberOfItemsPicked == _numberOfItemsToBeDelivered) return;
                Console.WriteLine("Picker():\tWaiting for rotator . . .");
                RotatorMutex.WaitOne();
                Console.WriteLine("Picker():\tRotator is free now.");
                Console.WriteLine($"Picker():\tPicking item #{_numberOfItemsPicked} . . .");
                if (!_picked) {
                    Console.WriteLine($"Picker():\tItem #{_numberOfItemsPicked} is picked.");
                    _picked = true;
                    _numberOfItemsPicked++;
                }
                else {
                    Console.WriteLine("Picker():\tNo item to be picked.");
                }
                RotatorMutex.ReleaseMutex();
                Thread.Sleep(1000);
            }
        }

        private static void Rotator() {
            while (true) {

                Console.WriteLine("Rotator():\tWaiting for loader and picker . . .");
                RotatorMutex.WaitOne();
                if (_loaded) {
                    Console.WriteLine("Rotator():\tItem is loaded on the left.");
                }
                if (_picked && _numberOfItemsPicked > 0) {
                    Console.WriteLine("Rotator():\tItem is picked on the right.");
                }
                if (_loaded && _picked) {
                    Console.WriteLine("========================================");
                    Console.WriteLine("Rotator():\tRotating . . . ");
                    _loaded = false;
                    _picked = false;
                    Console.WriteLine("Rotation completed.");
                    Console.WriteLine("========================================");
                }
                else {
                    if (!_loaded) {
                        Console.WriteLine("Rotator():\tWaiting for item to be loaded . . .");
                    }
                    if (!_picked) {
                        Console.WriteLine("Rotator():\tWaiting for item to be picked . . .");
                    }
                }
                RotatorMutex.ReleaseMutex();
                Thread.Sleep(1000);
            }
        }

        private static void Loader() {
            while (true) {
                if (_numberOfItemsLoaded == _numberOfItemsToBeDelivered) return;
                Console.WriteLine("Loader():\tWaiting for rotator . . .");
                RotatorMutex.WaitOne();
                Console.WriteLine("Loader():\tRotator is free now.");
                    Console.WriteLine($"Loader():\tLoading item #{_numberOfItemsLoaded} . . .");
                if (!_loaded) {
                    Console.WriteLine($"Loader():\tItem #{_numberOfItemsLoaded} is loaded.");
                    _loaded = true;
                    _numberOfItemsLoaded++;
                }
                else {
                    Console.WriteLine("Loader():\tCannot load item because rotator is loaded with an item already.");
                }
                Thread.Sleep(1000);
                RotatorMutex.ReleaseMutex();
            }

        }
    }
}
