var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();



var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.Run();