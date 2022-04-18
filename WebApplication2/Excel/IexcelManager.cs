using System.Data;

namespace WebApplication2.Excel;

public interface IExcelManager
{
    void WriteExcel(string fileName, DataTable table);
    DataTable ReadExcel(string fileName);
}