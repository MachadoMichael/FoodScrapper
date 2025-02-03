using Microsoft.EntityFrameworkCore;
using FoodScrapper.Infra.Database; 
using Microsoft.OpenApi.Models;
using FoodScrapper.Services;


var builder = WebApplication.CreateBuilder(args);

// Configuração de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Adiciona serviços ao contêiner
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuração do Entity Framework Core com PostgreSQL
builder.Services.AddDbContext<FoodDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("FoodDatabase")));

// Adiciona seus serviços
builder.Services.AddScoped<FoodService>();
builder.Services.AddScoped<ComponentService>();
builder.Services.AddScoped<ScrapperService>();

// Configuração do Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

// Configure o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAllOrigins");
app.MapControllers();
app.Run("http://0.0.0.0:4000");

