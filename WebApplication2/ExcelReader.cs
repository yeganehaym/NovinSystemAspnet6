namespace WebApplication2;

public class ExcelWriter
{
    public void WriteExcel(Action<int,int,string> designCell)
    {
        designCell(1, 1, "ali");
    }

    public void Math(Func<int, int, int> func)
    {
        var s=func(10, 5);
        Console.WriteLine("Math = " + s);
    }

    public string DateFormatter(Func<DateTime,string> dateFormatter=null)
    {
        if (dateFormatter != null)
            return dateFormatter(DateTime.Now);
        
        var date = DateTime.Now.ToString();
        return date;
    }
    
}