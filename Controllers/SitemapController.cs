using Microsoft.AspNetCore.Mvc;
using RecipeBackend.Services;

namespace RecipeBackend.Controllers;

[ApiController]
[Route("[Controller]")]
public class SitemapController : Controller
{
    public readonly ISitemapService _service;

    public SitemapController(ISitemapService service)
    {
        _service = service;
    }

    [HttpGet("random-url")]
    public async Task<string> Index()
    {
        string url = await _service.GetRandomUrlAsync();
        return url;
    }
}
