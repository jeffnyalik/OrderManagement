using Microsoft.EntityFrameworkCore;
using OrderManagement.Data;
using OrderManagement.Services;
using OrderManagement.Services.Discount;

var builder = WebApplication.CreateBuilder(args);

//Configure in -memory database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("OrderDb"));
builder.Services.AddScoped<IDiscountStrategy, NewCustomerDiscount>();
builder.Services.AddScoped<IDiscountStrategy, VipCustomerDiscount>();
builder.Services.AddScoped<DiscountService>();
builder.Services.AddScoped<OrderAnalyticsService>();
builder.Services.AddAutoMapper(typeof(Program).Assembly); // Register AutoMapper with the current assembly

// Add services to the container.

builder.Services.AddControllers();
   /* .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true; 

    });*/
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



var app = builder.Build();

using(var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // Seed the database with initial data
    SeedData.Initialize(db);
}

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
