using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MSUi.Data;
using MSUi.Services;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add database connection
var connectionString = builder.Configuration.GetConnectionString("UserConnection");
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(connectionString));


// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


// Add identitypour configurer l'infrastructure d'authentification et d'autorisation bas� sur les r�les et les users
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
   .AddEntityFrameworkStores<UserDbContext>()
    .AddDefaultTokenProviders();//--> fournisseur de jetons



builder.Services.AddScoped<IAuthService, AuthService>();
//configure l'authentification bas�e sur les jetons JWT (JSON Web Tokens) en utilisant le sch�ma d'authentification JWTBearer.
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

//--> ajoute le client HTTP dans le conteneur d'injection de d�pendances, ce qui permet � l' application de faire des requ�tes HTTP.
builder.Services.AddHttpClient();

//-->ajoute la prise en charge de la session dans l' application, ce qui permet de stocker des donn�es de session utilisateur entre les requ�tes HTTP.
builder.Services.AddSession();

//ajoute un accesseur pour acc�der � l'objet HttpContext dans votre application, ce qui permet d'acc�der � des informations
//sp�cifiques � la requ�te HTTP en cours.
builder.Services.AddHttpContextAccessor();

///ajoute le support pour les contr�leurs MVC avec les vues dans votre application, ce qui permet de cr�er des routes 
///et de g�rer les requ�tes HTTP entrantes en fonction des actions des contr�leurs.
builder.Services.AddControllersWithViews();

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
   //pattern: "{controller=Home}/{action=Index}/{id?}");
pattern: "{controller=Authentication}/{action=Login}");

app.Run();
