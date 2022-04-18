using System.Data;
using OfficeOpenXml;

namespace WebApplication2.Excel;

public class EpPlusReader:IExcelManager
{
    public void WriteExcel(string fileName, DataTable table)
    {
        throw new NotImplementedException();
    }

    public DataTable ReadExcel(string fileName)
    {
        var table = new DataTable();
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(fileName)))
        {
            var worksheet = excelPackage.Workbook.Worksheets[0];
                
            for (int i = 0; i < worksheet.Dimension.End.Column; i++)
            {
                table.Columns.Add();
            }
                
            //loop all rows
            for (int i = worksheet.Dimension.Start.Row; i <= worksheet.Dimension.End.Row; i++)
            {
                var row = table.NewRow();
                    
                //loop all columns in a row
                for (int j = worksheet.Dimension.Start.Column; j <= worksheet.Dimension.End.Column; j++)
                {
                    //add the cell data to the List
                    if (worksheet.Cells[i, j].Value != null)
                    {
                        row[j-1] = worksheet.Cells[i, j].Value.ToString();
                    }
                }
                table.Rows.Add(row);
            }
                
        }
        return table;
    }
}