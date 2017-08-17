using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OS_Assignment_Part_1_Mutex {
    class Program {
        static Mutex _mutex;
        static bool IsSingleInstance() {
            try {
                // Try to open existing mutex.
                Mutex.OpenExisting("PERL");
            }
            catch {
                // If exception occurred, there is no such mutex.
                Program._mutex = new Mutex(true , "PERL");

                // Only one instance.
                return true;
            }
            // More than one instance.
            return false;
        }

        static void Main() {
            while (true) {
                if (!Program.IsSingleInstance()) {
                    Console.WriteLine("More than one instance"); // Exit program.
                }
                else {
                    Console.WriteLine("One instance"); // Continue with program.
                }
                // Stay open.
                Console.ReadLine();
            }
        }
    }

}
