namespace Miners.Web.WebApp.Models;

public class ArticleModel
{
    public string Title { get; set; }
    public string Teaser { get; set; }
    public string Url { get; set; }
    public Guid ImageId { get; set; }
    public Guid Id { get; set; }
    public string Date { get; set; }
}