using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace MouseClickLogger
{
    class Program
    {
        // Import user32.dll for mouse hook functionality
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll")]
        private static extern int GetAsyncKeyState(int vKey);

        // Structure to hold mouse coordinates
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }
        static readonly int leftMouseButton = 0x01;
        static void Main(string[] args)
        {
            Console.WriteLine("Mouse Click Tracker - Started");
            Console.WriteLine("Click anywhere to record coordinates");
            Console.WriteLine("Press Enter to stop and save to file...");

            List<POINT> clickPoints = [];
            string outputFile = "clicks.txt";

            while (true)
            {
                if ((GetAsyncKeyState(leftMouseButton) & 0x8000) != 0)
                {
                    GetCursorPos(out POINT point);
                    clickPoints.Add(point);
                    Console.WriteLine($"Recorded: X={point.X}, Y={point.Y}");

                    Thread.Sleep(200);
                }

                if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    break;
                }

                Thread.Sleep(10);
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(outputFile))
                {
                    writer.WriteLine("X,Y");
                    foreach (var point in clickPoints)
                    {
                        writer.WriteLine($"{point.X},{point.Y}");
                    }
                }
                Console.WriteLine($"Saved {clickPoints.Count} points to {outputFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}