using System;
using System.Globalization;
using ola_automatica.worker.core.Interfaces;

namespace sodimac.cedis.core.Utils
{
    public class ConsolaSodimac : IConsolaSodimac
    {
        public void LogError(string value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{DateTime.Now.ToString("yy-MM-dd HH:mm:ss", new CultureInfo("es-ES"))}");
            Console.Write(" - Error : ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {value}");

        }
        public void LogCritical(string value, Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"{DateTime.Now.ToString("yy-MM-dd HH:mm:ss", new CultureInfo("es-ES"))}");
            Console.Write(" - Error : ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Ocurrio un error en el metodo: {value}");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($" {ex.Message}");

        }
        public void LogSuccess(string value)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{DateTime.Now.ToString("yy-MM-dd HH:mm:ss", new CultureInfo("es-ES"))}");
            Console.Write(" - Succ : ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {value}");

        }
        public void LogInfo(string value)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{DateTime.Now.ToString("yy-MM-dd HH:mm:ss", new CultureInfo("es-ES"))}");
            Console.Write(" - Info : ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {value}");

        }
        public void LogWarning(string value)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{DateTime.Now.ToString("yy-MM-dd HH:mm:ss", new CultureInfo("es-ES"))}");
            Console.Write(" - Warn : ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($" {value}");
        }

        public void Log(string value)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write($"{DateTime.Now.ToString("yy-MM-dd HH:mm:ss", new CultureInfo("es-ES"))}");
            Console.Write("        : ");
            Console.WriteLine($" {value}");
        }
        public void TextError(string value)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.White;

        }
        public void TextSuccess(string value)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.White;

        }
        public void TextInfo(string value)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.White;

        }
        public void TextWarning(string value)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
