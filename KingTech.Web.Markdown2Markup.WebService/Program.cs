using KingTech.Web.Markdown2Markup.WebService.Components;
using KingTech.Web.Markdown2Markup.WebService.Repositories;
using KingTech.Web.Markdown2Markup.WebService.Services;
using KingTech.Web.Markdown2Markup.WebService.Settings;

var builder = WebApplication.CreateBuilder(args);

//Get settings and add them to the container.
var generalSettings = builder.Configuration.GetSection(typeof(GeneralSettings).Name);
builder.Services.Configure<GeneralSettings>(generalSettings);

var fileSystemSettings = builder.Configuration.GetSection(typeof(FileSystemSettings).Name);
builder.Services.Configure<FileSystemSettings>(fileSystemSettings);

// Add services to the container.
builder.Services.AddTransient<IContentRepository, FileSystemContentRepository>();
builder.Services.AddTransient<IBlogService, BlogService>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
