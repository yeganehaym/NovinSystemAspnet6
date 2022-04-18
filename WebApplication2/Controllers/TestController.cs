using System.Data;
using System.Security.Claims;
using DNTPersianUtils.Core;
using ElmahCore;
using Hangfire;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using WebApplication2.Data;
using WebApplication2.Data.Domains;
using WebApplication2.Data.Entity;
using WebApplication2.Excel;
using WebApplication2.Messages;
using WebApplication2.Models.Products;
using WebApplication2.Options;
using WebApplication2.Services;

namespace WebApplication2.Controllers;

[Authorize]
public class TestController : Controller
{
    private ApplicationDbContext _applicationDbContext;
    private UserService _userService;
    private ProductServices _productServices;
    private IConfiguration _configuration;
    private ISmsManager _smsManager;
    private SmsService _smsService;
    private IWebHostEnvironment _environment;
    EmailOptions _emailOptions;

    public TestController(ApplicationDbContext applicationDbContext,UserService userService,
        ProductServices productServices, IConfiguration configuration, ISmsManager smsManager,
        SmsService smsService, IWebHostEnvironment environment, IOptionsSnapshot<EmailOptions> emailOptions)
    {
        _applicationDbContext = applicationDbContext;
        _userService = userService;
        _productServices = productServices;
        _configuration = configuration;
        _smsManager = smsManager;
        _smsService = smsService;
        _environment = environment;
        _emailOptions = emailOptions.Value;
        
    }
    public IActionResult Index()
    {
        SqlConnection connection = new SqlConnection("Server=.;Database=TestDb;User Id=sa;Password=123456789;");
        SqlCommand command = new SqlCommand("select * from products", connection);
        connection.Open();
        SqlDataReader reader = command.ExecuteReader();

        var list = new List<ProductGet>();
        while (reader.Read())
        {
            var product = new ProductGet();
            product.Id = int.Parse(reader["id"].ToString());
            product.Name = reader["name"].ToString();
            product.Description = reader["description"].ToString();
            product.Price = int.Parse(reader["price"].ToString());
            product.Image = reader["image"].ToString();
            list.Add(product);
        }
        connection.Close();
        return View(list);
    }

    public IActionResult List()
    {
        var list = _applicationDbContext
            .Products
            .Where(p=>p.Price>200000 || p.CreationDate>DateTime.Now)
            .OrderByDescending(x=>x.Price)
            .ThenBy(x=>x.Name)
            .ToList();
        
        var count = _applicationDbContext
            .Products
            .Count(p => p.Price>200000 || p.CreationDate>DateTime.Now);
        var max = _applicationDbContext
            .Products
            .Max(x=>x.Price);
        
        var productList = new ProductList();
        productList.Products = list;
        productList.ProductCount = count;
        productList.MaxPrice = max;
        return View(productList);
    }

    public IActionResult GetCount()
    {
        var count = _applicationDbContext
            .Products
            .Count();
        return Content("Count=" + count);
    }

    
    public IActionResult AddData()
    {
        var userId = User.GetUserId();
   

        _applicationDbContext.ProductServices.Add(new ProductService()
        {
            Name = "شست و شو بدنه خودرو",
            Price = 10000,
            Code = "1245",
            ProductType = ProductType.Service,
            UserId = userId
        });
        
        _applicationDbContext.ProductServices.Add(new ProductService()
        {
            Name = "نظافت داخلی خودرو",
            Price = 20000,
            Code = "1345",
            ProductType = ProductType.Service,
            UserId = userId
        });
        _applicationDbContext.ProductServices.Add(new ProductService()
        {
            Name = "اسفنج نانو",
            Price = 30000,
            Code = "1445",
            ProductType = ProductType.Product,
            UserId = userId
        });

        _applicationDbContext.Customers.Add(new Customer()
        {
            FirstName = "احمد",
            LastName = "زارع",
            Mobile = "09123456789"
        });

        _applicationDbContext.SaveChanges();
        return new EmptyResult();
    }

    public IActionResult NewInvoice(int id)
    {

        var invoice = new Invoice()
        {
            CustomerId = id,

        };
        var item1 = new InvoiceItem()
        {
            Invoice = invoice,
            ProductId = 1
        };
        var item2 = new InvoiceItem()
        {
            Invoice = invoice,
            ProductId = 2
        };
        var item3 = new InvoiceItem()
        {
            Invoice = invoice,
            ProductId =3
        };

        _applicationDbContext.Invoices.Add(invoice);
        _applicationDbContext.InvoiceItem.AddRange(item1,item2,item3);
        _applicationDbContext.SaveChanges();

        return new EmptyResult();
    }

    public async Task<IActionResult> TestUser()
    {
        var user = new User()
        {
            Username = "09323456789",
            MobileNumber = "09323456789",
            Password = "123456",
            FirstName = "ali",
            LastName = "yeganeh"
        };
        await _userService.AddUserAsync(user);

        await _productServices.AddProductAsync(new ProductService()
        {
            Name = "Test",
            Price = 400000,
        });
        await _applicationDbContext.SaveChangesAsync();
        return new EmptyResult();
    }

    public IActionResult Sms()
    {
        var apikey = _configuration["sms:ghasedak:apikey"];
        if (String.IsNullOrEmpty(apikey))
        {
            throw new Exception("SMS Api Key Is Empty");
        }
        return Content(apikey);
    }

    public IActionResult Cookie()
    {
        var cookie = Request.Cookies["testcookie"];
        return Content("Cookie Value:" + cookie);
    }
    
    public IActionResult WriteCookie()
    {
        Response.Cookies.Append("testcookie","this is a test cookie",new CookieOptions()
        {
            Expires = DateTimeOffset.Now.AddMinutes(1)
        });
        return Content("Cookie is Made");
    }

    [AllowAnonymous]
    public IActionResult TestString()
    {
        var x = new[] {"ali", "reza", "has,san"};
        var text = String.Join("-", x);
        var array = text.Split(new[] {'-', ','});
        return Content(text);
    }

    [AllowAnonymous]
    public IActionResult AddRole()
    {
        var role = new Role()
        {
            Name = "Hesabdar"
        };
        var role2 = new Role()
        {
            Name = "Anbardar"
        };
        _applicationDbContext.Roles.Add(role);
        _applicationDbContext.Roles.Add(role2);
        var userRole = new UserRole()
        {
            Role = role,
            UserId = 1005
        };
        var userRole2 = new UserRole()
        {
            Role = role2,
            UserId = 1005
        };
        _applicationDbContext.UserRoles.Add(userRole);
        _applicationDbContext.UserRoles.Add(userRole2);
        _applicationDbContext.SaveChanges();
        _applicationDbContext.SaveChanges();
        return new EmptyResult();
    }

    public async Task<IActionResult> ChangePassword()
    {
        var user =await _userService.FindUserAsync(1005);
        user.SerialNo = Utils.RandomString(Utils.RandomType.All, 10);
        await _applicationDbContext.SaveChangesAsync();
        return new EmptyResult();
    }

    public IActionResult ReadJsonData()
    {
        return View();
    }

    public IActionResult TestHangfire()
    {
        var jobId = BackgroundJob.Schedule(
            () => WriteFile() ,
            TimeSpan.FromMinutes(5));
        return Content("Job Id:" + jobId );
    }
    
    public IActionResult TestHangfire2()
    {
        RecurringJob.AddOrUpdate("Write File Every Minutes", () => WriteFile2(), Cron.Minutely);
        return new EmptyResult();
    }

    public void WriteFile()
    {
        var date = DateTime.Now.ToString("HH-mm");
        System.IO.File.WriteAllText("d:\\" + date + ".txt",date);
    }
    
    public void WriteFile2()
    {
        var date = DateTime.Now.ToString("HH-mm");
        System.IO.File.WriteAllText("d:\\recurring\\" + date + ".txt",date);
    }

    public IActionResult TestAction(string id)
    {
        var writer = new ExcelWriter();
        writer.WriteExcel((row, col, value) =>
        {
            Console.WriteLine("Row: " + row + "-Col: " + col + "-Value:" + value);
        });

        if (id == "+")
        {
            writer.Math((a, b) =>
            {
                return a + b;
            });
        }
        else if(id=="-")
        {
            writer.Math((a, b) =>
            {
                return a - b;
            });
        }
        else if (id == "*")
        {
            writer.Math((a, b) =>
            {
                return a * b;
            });
        }
        else
        {
            writer.Math((a, b) =>
            {
                return a / b;
            });
        }

        var date1 = writer.DateFormatter();
        var date2 = writer.DateFormatter(FormatDate);
        return Content("date1=" + date1 + ".Date2=" + date2);
    }

    private string FormatDate(DateTime dateTime)
    {
        return dateTime.ToPersianDateTextify();
    }

    public IActionResult MakeError()
    {
        try
        {
            var i = 3 - 3;
            var x = 4 / i;
            return Content(x.ToString());
        }
        catch (Exception e)
        {
            ElmahExtensions.RaiseError(e);
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<IActionResult> SendSmsByInterface()
    {
        /*ISmsManager smsManager = new KavehNegarSms();
        await smsManager.SendSmsAsync("0913", "test");
        smsManager = new GhsedakSmsService(_configuration);
        await smsManager.SendSmsAsync("0913", "test");*/
        var resul=await _smsManager.SendSmsAsync("0913", "test");
        
        return Content(resul);
    }
    public async Task<IActionResult> SendSmsByInterface2()
    {
        var smsManager = _smsService.GetSmsManger(1);
        var result = await smsManager.SendSmsAsync("0913", "test");
        return Content(result);
    }

    public IActionResult ReadExcelByEpPlus()
    {
        var excel = new EpPlusReader();
        var path = Path.Combine(_environment.WebRootPath, "nomre.xlsx");
        var table = excel.ReadExcel(path);

        var s = "";
        foreach (DataRow row in table.Rows)
        {
            var col1 = row[0].ToString();
            var col2 = row[1].ToString();
            var col3 = row[2].ToString();
            s += col1 + ":" + col2 + ":" + col3 + "-";
        }

        return Content(s);
    }
    
    public IActionResult ReadExcelByReader()
    {
        var excel = new EDR();
        var path = Path.Combine(_environment.WebRootPath, "nomre.xlsx");
        var table = excel.ReadExcel(path);

        var s = "";
        foreach (DataRow row in table.Rows)
        {
            var col1 = row[0].ToString();
            var col2 = row[1].ToString();
            var col3 = row[2].ToString();
            s += col1 + ":" + col2 + ":" + col3 + "-";
        }

        return Content(s);
    }

    public IActionResult WriteExcel()
    {
        var table = new DataTable();
        var col1 = new DataColumn("Date");
        col1.Caption = "تاریخ پرداخت";
            
        table.Columns.Add(col1);
        table.Columns.Add(new DataColumn("Time")
        {
            Caption = "ساعت پرداخت"
        });
        table.Columns.Add(new DataColumn("Amount",typeof(int))
        {
            Caption = "مبلغ (تومان)"
        });
        table.Columns.Add(new DataColumn("Description")
        {
            Caption = "توضیحات"
        });
        table.Columns.Add("Status");
        for (int i = 0; i < 5; i++)
        {
            var row = table.NewRow();
            row[0] = "1400/05/" + (i + 1).ToString("00");
            row[1] = "14:" + (i + 15).ToString("00");
            row[2] = (i + 1) * 5000;
            row[3] = "Description " + i;
            row[4] = i % 2 == 0 ? "پرداخت موفق" : "پرداخت ناموفق";
            table.Rows.Add(row);
        }
        
        var path = Path.Combine(_environment.WebRootPath, "test.xlsx");
        using (ExcelPackage package=new ExcelPackage())
        {
            var sheet = package.Workbook.Worksheets.Add("Payments");
            sheet.Cells.LoadFromDataTable(table,true,TableStyles.Light5);
                
                
            //رسم جدول از ستون و سط آ1
            // worksheet.Cells["A1"].LoadFromDataTable(excelOptions.DataTable,excelOptions.ShowColumnNames, excelOptions.TableStyles);
            //افزودن فوتر و هدر
            sheet.HeaderFooter.FirstHeader.CenteredText = "Header Text";
            sheet.HeaderFooter.FirstFooter.CenteredText = "Footer Text";
            sheet.Cells.AutoFitColumns(); 
            //کل سطر اول وسط چین شوند
            sheet.Row(1).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //همه سطرها وسط چین می شوند
            //sheet.Rows.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            //اضافه نمودن حقوق قانونی
                
            package.Workbook.Properties.Title = "payments";
            package.Workbook.Properties.Author = "HelpDesk 2";
            package.Workbook.Properties.Subject = "EPPlu Tutorial";
                
            //وضعیت نمایشی فایل اکسل هنگام باز شدن
            
            sheet.View.RightToLeft = true;
            //تعیین عرض ستون‌های جدول
            //sheet.Column(1).Width = 14;
            //sheet.Column(2).Width = 12;
                
            package.Compression = CompressionLevel.BestSpeed;
                
            package.SaveAs(path,"123456");
        }
        var fileStream = new FileStream(path, FileMode.Open);
            
        return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet","payment.xlsx");

    }

    public async Task<IActionResult> SendEmail()
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("NovinSystem", "info@novinsystem.com"));
        message.To.Add(new MailboxAddress("Ali yeganeh", "yeganehaym@gmail.com"));
        message.Subject = "Webinar 1400/01/30";

        message.Body = new TextPart(TextFormat.Html)
        {
            Text = @"Hey <strong>Chandler<strong>,

I just wanted to let you know that Monica and I were going to go play some paintball, you in?
<a href='https://www.google.com'>Go to Webinar</a>
-- Joey"
        };

        using (var client = new SmtpClient())
        {
            client.Connect(_emailOptions.Domain, _emailOptions.Port, false);

            // Note: only needed if the SMTP server requires authentication
            client.Authenticate(_emailOptions.Username, _emailOptions.Password);

            client.Send(message);
            client.Disconnect(true);
        }

        return Content("Email Sent");
    }
}