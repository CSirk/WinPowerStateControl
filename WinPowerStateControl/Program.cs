using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinPowerStateControl
{
    public static class Program
    {
        private static System.Threading.Timer _timer = null;
        private static bool _completeAction = false;
        private static long _selectedDuration = 0;
        static void Main(string[] args)
        {
            var validKeys = new string[] { "1", "2", "3", "Q", "q"};

            var selectedKey = "";
            while(selectedKey != "Q" && selectedKey != "q")
            {
                Console.WriteLine("Select command");
                Console.WriteLine("1 - Sleep");
                Console.WriteLine("2 - Hibernate");
                Console.WriteLine("3 - Shutdown");
                Console.WriteLine("Q - Exit");

                Console.WriteLine();
                Console.Write("Selected command : ");
                selectedKey = Console.ReadKey().KeyChar.ToString();
                Console.WriteLine();

                if(validKeys.Contains(selectedKey))
                {
                    var successfullyParsed = false;

                    Console.WriteLine("Select minutes before action (0 for instant)");
                    successfullyParsed = long.TryParse(Console.ReadKey().KeyChar.ToString(), out _selectedDuration);

                    if (successfullyParsed)
                    {
                        _timer = new System.Threading.Timer(TimerCallback, null, 0, 1000);
                        while (!_completeAction)
                        {
                            Thread.Sleep(1000);
                        }

                        switch(selectedKey)
                        {
                            case "1":
                                Application.SetSuspendState(PowerState.Suspend, true, true);
                                break;
                            case "2":
                                Application.SetSuspendState(PowerState.Hibernate, true, true);
                                break;
                            case "3":
                                Process.Start("shutdown", "/s /t 0");
                                break;
                        }
                        _timer.Dispose();
                        _completeAction = false;
                    }
                }
            }
        }

        private static void TimerCallback(Object o)
        {
            Console.WriteLine("Time before action : " + _selectedDuration);
            _selectedDuration -= 1;
            _completeAction = _selectedDuration <= 0;
        }
    }
}
