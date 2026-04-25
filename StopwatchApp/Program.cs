// using System.Diagnostics;

// namespace StopwatchApp;

// class Program
// {
//     static void Main()
//     {
//         Console.WriteLine("=== Stopwatch App ===");
//         Console.WriteLine("Commands: start, stop, reset, elapsed, exit");
//         Console.WriteLine();

//         Stopwatch stopwatch = new();

//         while (true)
//         {
//             Console.Write("> ");
//             string? command = Console.ReadLine()?.ToLower().Trim();

//             switch (command)
//             {
//                 case "start":
//                     stopwatch.Start();
//                     Console.WriteLine("✓ Stopwatch started");
//                     break;

//                 case "stop":
//                     stopwatch.Stop();
//                     Console.WriteLine("✓ Stopwatch stopped");
//                     break;

//                 case "reset":
//                     stopwatch.Restart();
//                     Console.WriteLine("✓ Stopwatch reset");
//                     break;

//                 case "elapsed":
//                     TimeSpan elapsed = stopwatch.Elapsed;
//                     Console.WriteLine($"Elapsed: {elapsed.Hours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}.{elapsed.Milliseconds:D3}");
//                     break;

//                 case "exit":
//                     Console.WriteLine("Goodbye!");
//                     return;

//                 default:
//                     Console.WriteLine("Unknown command. Use: start, stop, reset, elapsed, exit");
//                     break;
//             }
//         }
//     }
// }


using System;
using System.Threading;
using System.Threading.Tasks;

namespace StopwatchApp
{
    class Program
    {
        static bool isRunning = false;
        static TimeSpan elapsed = TimeSpan.Zero;
        static DateTime startTime;
        static Timer timer;
        static int cursorTop;

        static void Main(string[] args)
        {
            Console.Title = "Stopwatch - macOS";
            Console.CursorVisible = false;
            Console.Clear();

            DrawUI();

            timer = new Timer(UpdateDisplay, null, 0, 10);

            while (true)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.Spacebar:
                        ToggleStartStop();
                        break;
                    case ConsoleKey.R:
                        Reset();
                        break;
                    case ConsoleKey.Q:
                        timer?.Dispose();
                        Console.CursorVisible = true;
                        Console.Clear();
                        Console.WriteLine("Goodbye!");
                        return;
                }
            }
        }

        static void DrawUI()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════╗");
            Console.WriteLine("║           STOPWATCH              ║");
            Console.WriteLine("╠══════════════════════════════════╣");
            Console.WriteLine("║                                  ║");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("║         ");
            cursorTop = Console.CursorTop;
            Console.Write("00:00:00.00");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("          ║");
            Console.WriteLine("║                                  ║");
            Console.WriteLine("╠══════════════════════════════════╣");
            Console.WriteLine("║  [SPACE] Start/Stop              ║");
            Console.WriteLine("║  [R]     Reset                   ║");
            Console.WriteLine("║  [Q]     Quit                    ║");
            Console.WriteLine("╚══════════════════════════════════╝");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\nStatus: ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("STOPPED");
            Console.ResetColor();
        }

        static void UpdateDisplay(object state)
        {
            if (!isRunning) return;

            var current = DateTime.Now - startTime;
            var total = elapsed + current;

            lock (Console.Out)
            {
                int left = Console.CursorLeft;
                int top = Console.CursorTop;

                Console.SetCursorPosition(11, cursorTop);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"{total.Hours:D2}:{total.Minutes:D2}:{total.Seconds:D2}.{total.Milliseconds / 10:D2}");

                Console.SetCursorPosition(left, top);
            }
        }

        static void ToggleStartStop()
        {
            if (!isRunning)
            {
                startTime = DateTime.Now;
                isRunning = true;
                UpdateStatus("RUNNING", ConsoleColor.Green);
            }
            else
            {
                elapsed += DateTime.Now - startTime;
                isRunning = false;
                UpdateStatus("STOPPED", ConsoleColor.Red);
            }
        }

        static void Reset()
        {
            isRunning = false;
            elapsed = TimeSpan.Zero;

            lock (Console.Out)
            {
                int left = Console.CursorLeft;
                int top = Console.CursorTop;

                Console.SetCursorPosition(11, cursorTop);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("00:00:00.00");

                Console.SetCursorPosition(left, top);
            }

            UpdateStatus("STOPPED", ConsoleColor.Red);
        }

        static void UpdateStatus(string status, ConsoleColor color)
        {
            int left = Console.CursorLeft;
            int top = Console.CursorTop;

            Console.SetCursorPosition(8, cursorTop + 6);
            Console.ForegroundColor = color;
            Console.Write(status + "   ");

            Console.SetCursorPosition(left, top);
            Console.ResetColor();
        }
    }
}