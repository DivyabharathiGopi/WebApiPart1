using Microsoft.EntityFrameworkCore;
using QuestApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Db class injection
builder.Services.AddDbContext<QuestAppDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetSection("ConnectionString")["QuestAppConnectionString"]));
//builder.Services.AddDbContext<QuestAppDbContext>(options =>
//options.UseSqlServer(builder.Configuration.GetConnectionString("QuestAppConnectionString")));

var app = builder.Build();

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
