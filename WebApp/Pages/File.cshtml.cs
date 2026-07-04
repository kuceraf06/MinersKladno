using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Miners.Web.BusinessLayer;
using Miners.Web.BusinessLayer.Infrastructure;

namespace Miners.Web.WebApp.Pages;

public class File(AppDbContext dbContext) : PageModel
{
    public IActionResult OnGet(Guid id)
    {
        var fileEntity = dbContext.Files.Find(id);
        if (fileEntity == null || fileEntity.Data == null)
        {
            return NotFound();
        }
        return new FileContentResult(fileEntity.Data, MediaTypeNames.Application.Octet);
    }
}