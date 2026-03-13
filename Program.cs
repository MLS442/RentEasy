using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RentEasyAPI.Data;
using System.Text.Json.Serialization;

var allowReactApp = "_myReactApp";

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowReactApp,
                       policy =>
                       {
                           policy.WithOrigins("http://localhost:5173")
                                               .AllowAnyHeader()
                                               .AllowAnyMethod();
                       });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RentEasyContext>(options =>
                  options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);



builder.Services.AddControllers().AddJsonOptions(
    options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors(allowReactApp);

app.UseAuthorization();

app.MapControllers();

app.Run();
