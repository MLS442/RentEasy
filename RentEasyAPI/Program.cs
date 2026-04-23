using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RentEasyAPI.Data;
using RentEasyAPI.Services;
using Scalar.AspNetCore;
using System.Text;
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

// Validating the jwt
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["AppSettings:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["AppSettings:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["AppSettings:Token"]!)),
            ValidateIssuerSigningKey = true
        };
    });


builder.Services.AddControllers().AddJsonOptions(
    options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// ITicketService to TicketService
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

}

app.UseHttpsRedirection();

app.UseCors(allowReactApp);

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
