using RecipeBackend.Services;

var AllowFrontendOrigin = "_allowFrontendOrigin";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHttpClient<ISitemapService, SitemapService>();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowFrontendOrigin,
    builder =>
    {
        builder.WithOrigins("http://localhost:5173");
    });
});

var app = builder.Build();

app.UseCors(AllowFrontendOrigin);

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
