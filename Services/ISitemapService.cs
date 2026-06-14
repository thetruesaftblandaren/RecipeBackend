namespace RecipeBackend.Services;

public interface ISitemapService
{
    Task<string> GetRandomUrlAsync();
}
