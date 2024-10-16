using EduRateApi.Implementation;
using EduRateApi.Interfaces;
using FirebaseAdmin;
using UrbanLvivProjectAPI.Interfaces;
using UrbanLvivProjectAPI.Services;

var policy = "MyPolicy";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IFirebaseConnectingService, FirebaseConnectingService>();
// Add services to the container.
builder.Services.AddScoped<IShopService, ShopService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

// Configure the HTTP request pipeline.
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