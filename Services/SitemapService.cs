using System.Text.Json;
using System.Xml.Linq;
using RecipeBackend.Models;

namespace RecipeBackend.Services;

public class SitemapService : ISitemapService
{
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    private readonly List<SitemapConfig> _unfilteredSitemaps;
    private readonly List<SitemapConfig> _filteredSitemaps;

    private Random random = new();

    public SitemapService(IConfiguration config, HttpClient httpClient)
    {
        _config = config;
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("RecipeBackend");

        string json = File.ReadAllText("sitemaps.json");
        _unfilteredSitemaps = JsonSerializer.Deserialize<List<SitemapConfig>>(json) ?? new List<SitemapConfig>();
        _filteredSitemaps = _unfilteredSitemaps.Where(o => o.RecipesOnly == true).ToList();
    }

    public async Task<string> GetRandomUrlAsync(bool allowUnfiltered)
    {
        SitemapConfig sitemap;

        if (allowUnfiltered)
        {
            sitemap = _unfilteredSitemaps[random.Next(_unfilteredSitemaps.Count())];
        } else
        {
            sitemap = _filteredSitemaps[random.Next(_filteredSitemaps.Count())];
        }
        
        string sitemapStr = await _httpClient.GetStringAsync(sitemap.Url);

        XDocument doc = XDocument.Parse(sitemapStr);

        XNamespace ns = "http://www.sitemaps.org/schemas/sitemap/0.9";

        IEnumerable<XElement> urls;

        if (sitemap.Filter != null)
        {
            urls = doc.Descendants().Where(d => d.Name == ns + "loc" && d.Value.Contains(sitemap.Filter));
        }
        else
        {
            urls = doc.Descendants().Where(d => d.Name == ns + "loc");
        }
        string url = urls.ElementAt(random.Next(urls.Count())).Value.ToString();

        return url;
    }
}
