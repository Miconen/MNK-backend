using backend.Managers;
using backend.Data;
using Microsoft.EntityFrameworkCore;

// Add services to the container.
var builder = WebApplication.CreateBuilder(args);
string[] origins = { "http://localhost:8080", "https://localhost:8081" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder => policyBuilder
        .WithOrigins(origins)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());
});
builder.Services.AddControllers();
builder.Services.AddSignalR();
builder.Services.AddDbContext<DatabaseContext>(options => 
        options.UseNpgsql(builder.Configuration.GetConnectionString("DatabaseContext")));

// Configure the HTTP request pipeline.
var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/api/hub/chat");
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("/index.html");
});

app.UseStaticFiles();
app.Run();
