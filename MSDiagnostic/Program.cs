using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MSDiagnostic.Services;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


//autho authent
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = false,
               ValidateAudience = false,
               RequireExpirationTime = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = builder.Configuration.GetSection("JwtConfig:Issuer").Value,
               ValidAudience = builder.Configuration.GetSection("JwtConfig:Audience").Value,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

               //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
               //    .GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value))
           };
       });

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpClient();



// Register the IHttpClientFactory
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

//app.UseSession();
//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.Run();
