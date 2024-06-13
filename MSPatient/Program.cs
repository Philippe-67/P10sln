using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MSPatient.Data;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<PatientDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var server = Environment.GetEnvironmentVariable("DatabaseServer");
var port = Environment.GetEnvironmentVariable("DatabasePort");
var user = Environment.GetEnvironmentVariable("DatabaseUser");
var password = Environment.GetEnvironmentVariable("DatabasePassword");
var database = Environment.GetEnvironmentVariable("DatabaseName");

var connectionString = $"Server={server}, {port}; Initial Catalog = {database}; User ID = {user}; password ={password}; TrustServerCertificate=True";

builder.Services.AddDbContext<PatientDbContext>(options =>
   options.UseSqlServer(connectionString));


builder.Services.AddHttpClient();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                   .GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value))
           };
       });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
//app.UseSession();//necessaire
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
