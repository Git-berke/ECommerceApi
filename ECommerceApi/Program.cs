using ECommerceApi.Data;
using ECommerceApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer; // JWT için gerekli
using Microsoft.IdentityModel.Tokens; // JWT için gerekli
using System.Text; // Encoding.UTF8 için gerekli

var builder = WebApplication.CreateBuilder(args);

// DbContext'i servis koleksiyonuna ekliyoruz
builder.Services.AddDbContext<ECommerceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Servisleri baðýmlýlýk enjeksiyonuna ekliyoruz
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IAuthService, AuthService>(); // AuthService'i ekledik!

// JWT Kimlik Doðrulamasýný Yapýlandýrma
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Token'ý veren tarafý doðrula
        ValidateAudience = true, // Token'ý alýcý tarafý doðrula
        ValidateLifetime = true, // Token'ýn geçerlilik süresini doðrula
        ValidateIssuerSigningKey = true, // Ýmza anahtarýný doðrula
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

// Kimlik doðrulama ve yetkilendirme middleware'lerini ekleyin
// ÖNEMLÝ: UseRouting, UseCors (varsa) ve UseAuthentication/UseAuthorization sýralamasý kritiktir.
// Genellikle UseRouting'den sonra, UseEndpoints'ten önce gelirler.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();