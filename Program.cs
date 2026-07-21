using Radzen;
using TwilightImperiumRoller.Components;
using TwilightImperiumRoller.Services;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddBlazorBootstrap();
builder.Services.AddRadzenComponents();
builder.Services.AddRazorComponents()
  .AddInteractiveServerComponents();
builder.Services.AddSingleton<IGameStateRepository, JsonFileGameStateRepository>();
builder.Services.AddSingleton<IGameLogService, GameLogService>();
builder.Services.AddSingleton<IGameStateService, GameStateService>();
builder.Services.AddSingleton<IDiceRollingService, DiceRollingService>();
builder.Services.AddSingleton<IAgendaCatalogService, AgendaCatalogService>();
builder.Services.AddSingleton<IObjectiveCatalogService, ObjectiveCatalogService>();

WebApplication app = builder.Build();

await app.Services.GetRequiredService<IGameStateService>().InitializeAsync();
await app.Services.GetRequiredService<IAgendaCatalogService>().InitializeAsync();
await app.Services.GetRequiredService<IObjectiveCatalogService>().InitializeAsync();

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
