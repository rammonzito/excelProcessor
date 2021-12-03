using ExcelDataReader;
using ProcessExcel.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ProcessExcel
{
    public class ExcelProcessor : DocumentProcessor
    {
        readonly UsuariosModalImplementation ModalImplementation = new();
        readonly List<User> Users = new();
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

                ConfigReading(out double col, out int row);

                var dataSet = reader.AsDataSet(conf);
                var dataTable = dataSet.Tables[0];

                PrepareInfo(row, dataTable);
            }

            ModalImplementation.Process(Users);
        }

        #region privates 
        private static void ConfigReading(out double col, out int row)
        {
            var cellStr = "A1";
            var match = Regex.Match(cellStr, @"(?<col>[A-Z]+)(?<row>\d+)");
            var colStr = match.Groups["col"].ToString();
            col = colStr.Select((t, i) => (colStr[i] - 64) * Math.Pow(26, colStr.Length - i - 1)).Sum();
            row = 0;
        }

        private void PrepareInfo(int row, DataTable dataTable)
        {
            
            for (var i = row; i < dataTable.Rows.Count; i++)
            {
                DataRow data = dataTable.Rows[i];
                Users.Add(ModalImplementation.Prepare(data));
            }
        }
        #endregion
    }
}
