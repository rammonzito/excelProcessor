using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProcessExcel
{
    public class MyLog
    {
        public static void SaveLog(StringBuilder sb)
        {
            File.AppendAllText($"{Assembly.GetExecutingAssembly().Location}-NotCreated.txt", sb.ToString());
            sb.Clear();
        }
    }
}
