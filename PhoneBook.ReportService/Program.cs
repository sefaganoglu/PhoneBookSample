using Microsoft.EntityFrameworkCore;
using PhoneBook.Library.Services;
using PhoneBook.ReportService.BackgroundServices;
using PhoneBook.ReportService.DAL;
using PhoneBook.ReportService.Services;
using PhoneBook.ReportService.Services.Configurations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ReportDbContext>(option => option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<INLogService, NLogService>();

builder.Services.AddSingleton(new ContactApiConfig { BaseUrl = builder.Configuration.GetValue<string>($"{ nameof(ContactApiConfig) }:BaseUrl") });
builder.Services.AddSingleton<ContactApiService>();

builder.Services.AddSingleton(new RabbitMQConfig { RabbitMQConnection = builder.Configuration.GetValue<string>($"{ nameof(RabbitMQConfig) }:RabbitMQConnection") });
builder.Services.AddSingleton<RabbitMQService>();

builder.Services.AddHostedService<ReportBuilder>();

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

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ReportDbContext>();
    dbContext.Database.Migrate();
}

app.Run();
