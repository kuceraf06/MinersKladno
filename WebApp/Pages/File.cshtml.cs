using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Miners.Web.BusinessLayer;

namespace Miners.Web.WebApp.Pages;

public class File(AppDbContext dbContext) : PageModel
{
    public IActionResult OnGet(Guid id)
    {
        var fileEntity = dbContext.Files.Find(id);
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles", fileEntity.Filename);
        if (!System.IO.File.Exists(filePath))
        {
            return NotFound();
        }

        return new PhysicalFileResult(filePath, MediaTypeNames.Application.Octet);
    }
}