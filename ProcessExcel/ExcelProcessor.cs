using ExcelDataReader;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ProcessExcel
{
    public class ExcelProcessor : DocumentProcessor
    {
        public override void Execute(string path)
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.ReadWrite))
            {
                IExcelDataReader reader;
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                reader = ExcelReaderFactory.CreateReader(stream);
                var conf = new ExcelDataSetConfiguration
                {
                    ConfigureDataTable = _ => new ExcelDataTableConfiguration
                    {
                        UseHeaderRow = true
                    }
                };

                double col;
                int row;
                ConfigReading(out col, out row);

                var dataSet = reader.AsDataSet(conf);
                var dataTable = dataSet.Tables[0];

                ForEachInfo(row, dataTable);
            }
        }

        private static void ConfigReading(out double col, out int row)
        {
            var cellStr = "A1";
            var match = Regex.Match(cellStr, @"(?<col>[A-Z]+)(?<row>\d+)");
            var colStr = match.Groups["col"].ToString();
            col = colStr.Select((t, i) => (colStr[i] - 64) * Math.Pow(26, colStr.Length - i - 1)).Sum();
            row = int.Parse(match.Groups["row"].ToString());
        }

        private void ForEachInfo(int row, DataTable dataTable)
        {
            var modalImplementation = new UsuariosModalImplementation();
            for (var i = row; i < dataTable.Rows.Count; i++)
            {
                DataRow data = dataTable.Rows[i];
                var exists = modalImplementation.Process(data);
            }
        }
    }
}
