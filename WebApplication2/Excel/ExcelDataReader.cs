using System.Data;
using ExcelDataReader;

namespace WebApplication2.Excel;

public class EDR:IExcelManager
{
    public void WriteExcel(string fileName, DataTable table)
    {
        throw new NotImplementedException();
    }

    public DataTable ReadExcel(string fileName)
    {
        var table = new DataTable();
        using (var stream = File.Open(fileName, FileMode.Open, FileAccess.Read))
        {
            // Auto-detect format, supports:
            //  - Binary Excel files (2.0-2003 format; *.xls)
            //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                // Choose one of either 1 or 2:
                // 1. Use the reader methods
                var colCount = reader.FieldCount;
                for (int i = 0; i < colCount; i++)
                {
                    table.Columns.Add();
                }
                do
                {
                        
                    while (reader.Read())
                    {
                        var row = table.NewRow();
                        for (int i = 0; i < colCount; i++)
                        {
                            if (reader.GetValue(i) != null)
                            {
                                row[i] = reader.GetValue(i).ToString();
                            }
                        }
                        table.Rows.Add(row);
                        // reader.GetDouble(0);
                    }
                } while (reader.NextResult());
            }
        }
        return table;
    }
}