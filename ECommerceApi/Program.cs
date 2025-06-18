using ECommerceApi.Data; // ECommerceDbContext s�n�f�n�z� kullanabilmek i�in ekledik
using Microsoft.EntityFrameworkCore; // AddDbContext ve UseSqlServer metotlar� i�in ekledik
using ECommerceApi.Data;
using ECommerceApi.Services;

var builder = WebApplication.CreateBuilder(args);

// DbContext'i servis koleksiyonuna ekliyoruz
builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ProductService'i ba��ml�l�k enjeksiyonuna ekliyoruz
// AddScoped: Her HTTP iste�i i�in ProductService'in yeni bir �rne�i olu�turulur.
builder.Services.AddScoped<IProductService, ProductService>();



// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();