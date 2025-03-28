using System;
using System.IO;
using Gma.System.MouseKeyHook;

namespace MouseClickLogger
{
    class MouseClickLogger
    {
        private static IKeyboardMouseEvents mouseEvents;
        private string logFilePath = "MouseClicksLog.txt";

        public static void StartLogging()
        {
            mouseEvents = Hook.GlobalEvents();

            mouseEvents.MouseDown += OnMouseDown;

            Console.WriteLine("Mouse click logger started. Press ENTER to stop...");
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            string logEntry = $"{DateTime.Now}: Mouse clicked at ({e.X}, {e.Y})";
            Console.WriteLine(logEntry);

            File.AppendAllLines(logFilePath, new[] { logEntry });
        }

        public static void StopLogging()
        {
            if (mouseEvents != null)
            {
                mouseEvents.MouseDown -= OnMouseDown;
                mouseEvents.Dispose();
            }

            Console.WriteLine("Mouse click logger stopped.");
        }

        static void Main(string[] args)
        {
            var logger = new MouseClickLogger();

            logger.StartLogging();

            Console.ReadLine();

            logger.StopLogging();
        }
    }
}
