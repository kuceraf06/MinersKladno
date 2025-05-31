namespace Miners.Web.WebApp.Models;

public class CarouselArticleModel
{
    public Guid Id { get; set; }
    public Guid ImageId { get; set; }
    public string Title { get; set; }
    public DateTime DateCreated { get; set; }
    public string Teaser { get; set; }
}