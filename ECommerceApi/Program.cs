using ECommerceApi.Data;
using ECommerceApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer; // JWT i�in gerekli
using Microsoft.IdentityModel.Tokens; // JWT i�in gerekli
using System.Text; // Encoding.UTF8 i�in gerekli

var builder = WebApplication.CreateBuilder(args);

// DbContext'i servis koleksiyonuna ekliyoruz
builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servisleri ba��ml�l�k enjeksiyonuna ekliyoruz
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthService, AuthService>(); // AuthService'i ekledik!

// JWT Kimlik Do�rulamas�n� Yap�land�rma
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Token'� veren taraf� do�rula
        ValidateAudience = true, // Token'� al�c� taraf� do�rula
        ValidateLifetime = true, // Token'�n ge�erlilik s�resini do�rula
        ValidateIssuerSigningKey = true, // �mza anahtar�n� do�rula
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // appsettings'ten oku
        ValidAudience = builder.Configuration["Jwt:Audience"], // appsettings'ten oku
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])) // appsettings'ten oku
    };
});

// Yetkilendirme (Authorization) servisini ekle
builder.Services.AddAuthorization();


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

// Kimlik do�rulama ve yetkilendirme middleware'lerini ekleyin
// �NEML�: UseRouting, UseCors (varsa) ve UseAuthentication/UseAuthorization s�ralamas� kritiktir.
// Genellikle UseRouting'den sonra, UseEndpoints'ten �nce gelirler.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();