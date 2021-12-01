using ProcessExcel;
using System;
using System.IO;
using System.Reflection;

namespace ClientDocumentProcessor
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            ExcelProcessor processor = new ExcelProcessor();
            processor.Execute($"{typeof(Program).Assembly.GetDirectoryPath()}/Base Beneficio Eleven.xlsx");
        }

        public static string GetDirectoryPath(this Assembly assembly)
        {
            string filePath = new Uri(assembly.CodeBase).LocalPath;
            return Path.GetDirectoryName(filePath);
        }
    }
}
