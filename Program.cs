using ecommerce.data;
using ecommerce.data.postgre;
using ecommerce.service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using ecommerce.extension;

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

builder.Services.AddSingleton<IBookRepository, BookRepository>();
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();

builder.Services.AddSingleton<ITokenService,TokenService>();
builder.Services.AddSingleton<IBookService, BookService>();
builder.Services.AddSingleton<IOrderService, OrderService>();
builder.Services.AddSingleton<ICustomerService, CustomerService>();

builder.AddAutherization(settings);

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
else
{
    //auto migrate db
    using (var scope = app.Services.CreateScope()) // Change this line
    {
        using (var context = scope.ServiceProvider.GetService<AppDbContext>())
        {
            context.Database.Migrate();
        }
    }

}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
