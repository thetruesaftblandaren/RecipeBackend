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
    public async Task<string> Index([FromQuery]bool allowUnfiltered = true)
    {
        string url = await _service.GetRandomUrlAsync(allowUnfiltered);
        return url;
    }
}
