using Tienda_Celulares.Web;
using Tienda_Celulares.Web.Components;
using Tienda_Celulares.Web.Services;

var builder = WebApplication.CreateBuilder(args);

//Registrar servicio
builder.Services.AddScoped<ProductoService>();

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOutputCache();


// En Tienda_Celulares.Web -> Program.cs
builder.Services.AddScoped(sp => new HttpClient
{
    // Reemplaza el puerto (7357) por el que use tu ApiService al iniciar
    BaseAddress = new Uri("https://localhost:7473/")
});

//builder.Services.AddScoped<ClienteService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseOutputCache();

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
