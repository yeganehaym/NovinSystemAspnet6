using System.Collections;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Data;
using WebApplication2.Data.Entity;
using WebApplication2.Models;
using WebApplication2.Models.Customers;

namespace WebApplication2.Controllers;

public class CustomerController : Controller
{
    private ApplicationDbContext _context;
    // GET
    public CustomerController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> LoadCustomers(DataTableParams dataTableParams,int type)
    {
        var query = _context.Customers
            .AsNoTracking()
            .AsQueryable();

        if (type >= 0)
        {
            var enumType = (CustomerType) type;
            query = query.Where(x => x.CustomerType == enumType);
            
        }
    
        var customers = await query
            .OrderByDescending(x => x.Id)
            .Skip(dataTableParams.Start)
            .Take(dataTableParams.Length)
            .ToListAsync();
        
        var totalCount = await _context
            .Customers
            .CountAsync();

      /*  var items = customers
            .Select(x => new CustomerGet()
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Mobile = x.Mobile,
                CustomerType =(int) x.CustomerType
            })
            .ToList();*/

      
        var items = customers.Adapt<List<CustomerGet>>();
        return Json(new DataTableResult<CustomerGet>()
        {
            Draw = dataTableParams.Draw,
            Data = items,
            RecordsFiltered = totalCount,
            RecordsTotal = totalCount
        });
    }
}