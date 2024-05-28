using Microsoft.EntityFrameworkCore;
using MSPatient.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PatientDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddSession();


// Add services to the container.

builder.Services.AddControllers();
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
//app.UseSession();//necessaire
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
