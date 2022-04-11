using System.Security.Claims;
using Mapster;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebApplication2;
using WebApplication2.Data;
using WebApplication2.Data.Entity;
using WebApplication2.Models.Customers;
using WebApplication2.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("default"));
});


builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ProductServices>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login";
        options.LogoutPath = "/auth/logout";
        options.AccessDeniedPath = "/auth/login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
        options.SlidingExpiration = true;
        options.Events = new CookieAuthenticationEvents()
        {
            OnValidatePrincipal = async context =>
            {
                var claim = context.Principal.Claims.First(x => x.Type == ClaimTypes.NameIdentifier);
                var serialNo = context.Principal.Claims.First(x => x.Type == ClaimTypes.SerialNumber).Value;
                var userId = int.Parse(claim.Value);
              
                var userService = context.HttpContext.RequestServices.GetService<UserService>();
                var user = await userService.FindUserAsync(userId);
                //if (user.IsAdmin == false)
                    //context.RejectPrincipal();
                if (serialNo != user.SerialNo)
                {
                    context.RejectPrincipal();
                }
            }
        };

    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


TypeAdapterConfig.GlobalSettings
    .NewConfig<Customer, CustomerGet>()
    .Ignore(dest=>dest.Id)
    .Map(dest => dest.InsertTime, src => src.CreationDate);

app.Run();