using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Miners.Web.BusinessLayer;

var builder = WebApplication.CreateBuilder(args);

var czechCulture = new CultureInfo("cs-CZ");

// Set it as default culture for the entire application
CultureInfo.DefaultThreadCurrentCulture = czechCulture;
CultureInfo.DefaultThreadCurrentUICulture = czechCulture;


builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddRazorPages();


var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.Run();