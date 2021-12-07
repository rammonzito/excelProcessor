using ProcessExcel;
using ProcessExcel.Contants;
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
            processor.Execute($"{typeof(Program).Assembly.GetDirectoryPath()}/{MainConstants.FileName}");
        }

        private static string GetDirectoryPath(this Assembly assembly)
        {
            string filePath = new Uri(assembly.CodeBase).LocalPath;
            return Path.GetDirectoryName(filePath);
        }
    }
}
