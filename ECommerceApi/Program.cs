using ECommerceApi.Data; // ECommerceDbContext sýnýfýnýzý kullanabilmek için ekledik
using Microsoft.EntityFrameworkCore; // AddDbContext ve UseSqlServer metotlarý için ekledik
using ECommerceApi.Data;
using ECommerceApi.Services;

var builder = WebApplication.CreateBuilder(args);

// DbContext'i servis koleksiyonuna ekliyoruz
builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ProductService'i baðýmlýlýk enjeksiyonuna ekliyoruz
// AddScoped: Her HTTP isteði için ProductService'in yeni bir örneði oluþturulur.
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