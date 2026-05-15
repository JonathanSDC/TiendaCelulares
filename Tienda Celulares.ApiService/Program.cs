using Microsoft.EntityFrameworkCore;
using Tienda_Celulares.ApiService.Data;
using Tienda_Celulares.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

//Configurar JSON para mantener los nombres de las propiedades tal cual están en el modelo
builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.PropertyNamingPolicy = null;

    // Evita el error 500 por bucles infinitos en las relaciones
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

//Configurar CORS
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

//Registrar DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

//Registrar servicios personalizados
builder.Services.AddScoped<AuthService>();

//Configurar JSON para evitar ciclos de referencia
builder.Services.AddControllers()
    .AddJsonOptions(options => {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Activar Controllers
//builder.Services.AddControllers();






var app = builder.Build();

//Configurar CORS
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

//Activar Controllers
app.MapControllers();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseDeveloperExceptionPage();
}




app.MapDefaultEndpoints();

app.Run();

