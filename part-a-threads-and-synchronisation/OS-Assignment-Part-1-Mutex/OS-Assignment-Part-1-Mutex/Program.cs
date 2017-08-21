﻿using System;
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
        private static Mutex _rotatorMutex = new Mutex();
        private static bool _loaded = false;
        private static bool _picked = false;
        private static bool _havingItem = false;
        private static bool _rotated = false;

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
                if (!_picked && _havingItem && _rotated) {
                    Console.WriteLine("Picker() : Picking item . . .");
                    _picked = true;
                    _havingItem = false;
                    _rotated = false;
                }
                Thread.Sleep(1000);
                _rotatorMutex.ReleaseMutex();
            }
        }

        private static void Rotator() {
            while (true) {
                Console.WriteLine("Rotator() : Waiting for loader and picker . . .");
                _rotatorMutex.WaitOne();
                Console.WriteLine("Rotator() : Rotating . . . ");
                if(_loaded && _picked)
                {
                    Console.WriteLine("Loader() : Loading item . . .");
                    _loaded = false;
                    _picked = false;
                    _rotated = true;
                }
                Thread.Sleep(1000);
                _rotatorMutex.ReleaseMutex();
            }
        }

        private static void Loader() {
            while (true) {
                Console.WriteLine("Loader() : Waiting for rotator . . .");
                _rotatorMutex.WaitOne();
                if (!_loaded && !_havingItem) {
                    Console.WriteLine("Loader() : Loading item . . .");
                    _loaded = true;
                    _havingItem = true;
                }
                Thread.Sleep(1000);
                _rotatorMutex.ReleaseMutex();
            }

        }
    }
}
