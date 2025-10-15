using AdventureWorks.Enterprise.App.Components;
using AdventureWorks.Enterprise.App.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configurar HttpClient para el API
var StrApiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7279";
var StrApiKey = builder.Configuration["ApiSettings:ApiKey"] ?? "";

builder.Services.AddHttpClient<ApiService>(client =>
{
    client.BaseAddress = new Uri(StrApiBaseUrl);
    client.DefaultRequestHeaders.Add("X-Api-Key", StrApiKey);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.Timeout = TimeSpan.FromSeconds(30);
});

// Registrar ApiService
builder.Services.AddScoped<ApiService>();

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
