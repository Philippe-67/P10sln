using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddSession();



// Add ocelot configuration
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);
// Add jwt bearer
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

// Ajouter la configuration du service IDistributedCache
builder.Services.AddDistributedMemoryCache(); // Vous pouvez utiliser une mise en cache en mémoire pour les besoins de développement

// Ajouter la gestion des sessions
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Définir le délai d'expiration de la session
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseOcelot().Wait();

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
