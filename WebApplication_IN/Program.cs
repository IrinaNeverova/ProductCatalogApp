using Microsoft.EntityFrameworkCore;
using WebApplication_IN.Models;
using Microsoft.OpenApi.Models;
using WebApplication_IN.ImportXmlFeed;
using WebApplication_IN.Service;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
//.ConfigureApiBehaviorOptions(options =>
// {
//     options.SuppressMapClientErrors = true;
// });


// Add services to the container.
builder.Services.AddHostedService<ProductImport>();
//builder.Services.AddScoped<ProductImport>();
builder.Services.AddScoped<ProductService>();
//builder.Services.AddScoped<ProductContext>();
//builder.Services.AddScoped<IProductService>();


var connectionString = builder.Configuration.GetConnectionString("ProductContext");
builder.Services.AddDbContext<ProductContext>(options => options.UseSqlServer(connectionString));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo
        {
            Title = "WebApplication_IN - V1",
            Description = "Description Swagger",
            Version = "v1"
        }
     );

    var filePath = Path.Combine(System.AppContext.BaseDirectory, "WebApplication_IN.xml");
    c.IncludeXmlComments(filePath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplication_IN V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
