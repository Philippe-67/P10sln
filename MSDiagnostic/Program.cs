using MSDiagnostic.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddSession();


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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
