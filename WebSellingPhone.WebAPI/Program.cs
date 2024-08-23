using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebSellingPhone.Bussiness.Service;
using WebSellingPhone.Data;
using WebSellingPhone.Data.Infrastructure;
using WebSellingPhone.Data.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.   

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PhoneWebDbContext>(option =>
{
    option.UseSqlServer(connectionString);
});


builder.Services.AddControllers();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<IPromotionService, PromotionService>();
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
