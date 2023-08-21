using ecommerce.data;
using ecommerce.data.postgre;
using ecommerce.service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookingStoreAPI", Version = "v1" });
});


Settings settings = new Settings(builder.Configuration);
builder.Services.AddSingleton<ISettings>(services => settings);


//postgre db configuration
var dbContextPostgre=new ecommerce.data.postgre.AppDbContext( settings);
builder.Services.AddSingleton<ecommerce.data.postgre.AppDbContext>(services=>dbContextPostgre);
builder.Services.AddSingleton<ICustomerRepository, ecommerce.data.postgre.CustomerRepository>();

builder.Services.AddSingleton<TokenService, TokenService>();


builder.Services.AddSingleton<ICustomerService, CustomerService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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

app.UseAuthorization();

app.MapControllers();

app.Run();
