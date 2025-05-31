// See https://aka.ms/new-console-template for more information

using Microsoft.EntityFrameworkCore;
using Miners.Web.BusinessLayer;

var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
optionsBuilder.UseNpgsql("Server=172.205.192.35;Database=miners;Port=5432;User Id=usr_miners;Password=QWxDcXc&hfACRFPE8WP;");
var dbContext = new AppDbContext(optionsBuilder.Options);

var files = dbContext.Files.Where(x=>x.Data == null).ToList();
foreach (var file in files)
{
    try
    {
        var fsFile = File.OpenRead(@"d:\temp\uploadedfiles\" + file.Filename);
        file.Data = new byte[fsFile.Length];
        fsFile.Read(file.Data, 0, (int)fsFile.Length);
        fsFile.Close();
        dbContext.Files.Update(file);
        dbContext.SaveAndCommit();
    }
    catch{ }
}
Console.WriteLine($"Successfully uploaded files: {files.Count}");