
using FirebaseAdmin;
using UrbanLvivProjectAPI.Interfaces;
using UrbanLvivProjectAPI.Services;

var policy = "MyPolicy";
var builder = WebApplication.CreateBuilder(args);

IWebHostEnvironment env = builder.Environment;
string environment = Environment.GetEnvironmentVariable("enviroment") ?? "Development";

builder.Configuration
    .SetBasePath(env.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();

builder.Services.AddScoped<IFirebaseConnectingService, FirebaseConnectingService>();
builder.Services.AddScoped<IReportService, ReportService>();


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: policy, policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors(policy);
app.Run();