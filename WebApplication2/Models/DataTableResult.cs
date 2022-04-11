namespace WebApplication2.Models;

public class DataTableResult<T> where T : class
{
    public  int Draw { get; set; }
    public int RecordsTotal { get; set; }
    public int RecordsFiltered { get; set; }
    public List<T> Data { get; set; }
}