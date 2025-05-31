using System.Text.RegularExpressions;

namespace Miners.Web.BusinessLayer.Services;

public class ArticleService
{
    public static string CreateUrl(string text, int id)
    {
        if (string.IsNullOrEmpty(text)) return "";

        // Convert to lowercase
        text = text.ToLowerInvariant();

        // Replace Czech/Slovak characters
        var map = new Dictionary<char, char>
        {
            { 'á', 'a' }, { 'č', 'c' }, { 'ď', 'd' }, { 'é', 'e' }, { 'ě', 'e' },
            { 'í', 'i' }, { 'ň', 'n' }, { 'ó', 'o' }, { 'ř', 'r' }, { 'š', 's' },
            { 'ť', 't' }, { 'ú', 'u' }, { 'ů', 'u' }, { 'ý', 'y' }, { 'ž', 'z' }
        };

        text = new string(text.Select(c => map.ContainsKey(c) ? map[c] : c).ToArray());

        // Replace spaces with hyphens and remove invalid characters
        text = Regex.Replace(text, @"[^a-z0-9\s-]", "");
        text = Regex.Replace(text, @"\s+", "-");
        text = Regex.Replace(text, @"-+", "-");

        return id + "-" + text.Trim('-');
    }

    public static string CreateTeaser(string markdownContent, int maxLength = 200)
    {
        if (string.IsNullOrEmpty(markdownContent))
            return string.Empty;

        // Remove Markdown headings
        var text = Regex.Replace(markdownContent, @"^#{1,6}\s.*$", "", RegexOptions.Multiline);
        
        // Remove Markdown links but keep the text
        text = Regex.Replace(text, @"\[([^\]]+)\]\([^)]+\)", "$1");
        
        // Remove Markdown images
        text = Regex.Replace(text, @"!\[([^\]]*)\]\([^)]+\)", "");
        
        // Remove Markdown bold and italic
        text = Regex.Replace(text, @"[*_]{1,2}([^*_]+)[*_]{1,2}", "$1");
        
        // Remove code blocks
        text = Regex.Replace(text, @"```[\s\S]*?```", "");
        
        // Remove inline code
        text = Regex.Replace(text, @"`[^`]+`", "");
        
        // Remove blockquotes
        text = Regex.Replace(text, @"^>\s.*$", "", RegexOptions.Multiline);
        
        // Remove horizontal rules
        text = Regex.Replace(text, @"^[-*_]{3,}$", "", RegexOptions.Multiline);
        
        // Remove HTML tags if any
        text = Regex.Replace(text, "<[^>]+>", "");
        
        // Remove extra whitespace and empty lines
        text = Regex.Replace(text, @"\s+", " ");
        text = text.Trim();

        // If the text is shorter than maxLength, return it all
        if (text.Length <= maxLength)
            return text;

        // Find the last space before maxLength
        var lastSpace = text.LastIndexOf(' ', maxLength);
        if (lastSpace == -1)
            lastSpace = maxLength;

        // Return the truncated text with ellipsis
        return text[..lastSpace].Trim() + "...";
    }
}