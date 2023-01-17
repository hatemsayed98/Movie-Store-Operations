using Microsoft.EntityFrameworkCore;
using MovieMarket.Models;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("conn");
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlServer(connectionString));
// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddMvc().AddNToastNotifyToastr(new NToastNotify.ToastrOptions()
{
    ProgressBar = true,
    PositionClass = "toast-top-right",
    PreventDuplicates = true, 
    CloseButton = true,
    
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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
