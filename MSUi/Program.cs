using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MSUi.Data;
using MSUi.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
//--> ajoute le client HTTP dans le conteneur d'injection de dépendances, ce qui permet à l' application de faire des requêtes HTTP.
builder.Services.AddHttpClient();



// Add database connection sans docker
////var connectionString = builder.Configuration.GetConnectionString("UserConnection");
////builder.Services.AddDbContext<UserDbContext>(options =>
////    options.UseSqlServer(connectionString));


//connection docker
var server = Environment.GetEnvironmentVariable("DatabaseServer");
var port = Environment.GetEnvironmentVariable("DatabasePort");
var user = Environment.GetEnvironmentVariable("DatabaseUser");
var password = Environment.GetEnvironmentVariable("DatabasePassword");
var database = Environment.GetEnvironmentVariable("DatabaseName");

var connectionString = $"Server={server}, {port}; Initial Catalog = {database}; User ID = {user}; password ={password}; TrustServerCertificate=True";

builder.Services.AddDbContext<UserDbContext>(options =>
   options.UseSqlServer(connectionString));

//// Add identitypour configurer l'infrastructure d'authentification et d'autorisation basé sur les rôles et les users
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
   .AddEntityFrameworkStores<UserDbContext>()
   .AddDefaultTokenProviders();//--> fournisseur de jetons

//// Add interface
builder.Services.AddScoped<IAuthService, AuthService>();

//configure l'authentification basée sur les jetons JWT (JSON Web Tokens) en utilisant le schéma d'authentification JWTBearer.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(180); // Durée de validité du cookie
            options.SlidingExpiration = true; // Renouvelle le cookie à chaque requête si l'utilisateur est actif
        })

       .AddJwtBearer(options =>
       {
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               RequireExpirationTime = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
               ValidAudience = builder.Configuration.GetSection("JwtCAudience").Value,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                                                   .GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value))
           };
       });




// Add session
builder.Services.AddSession();
// Add http context accessor
builder.Services.AddHttpContextAccessor();





// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();



//configuration des logs
var configuration = builder.Configuration;
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.Console()
    .CreateLogger();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
  // pattern: "{controller=Home}/{action=Index}/{id?}");
pattern: "{controller=Authentication}/{action=Login}");

app.Run();
