using backend.Hubs;

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
builder.Services.AddSignalR();
builder.Services.AddControllers();

// Configure the HTTP request pipeline.
var app = builder.Build();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/chat");
    endpoints.MapControllers();
    endpoints.MapFallbackToFile("/index.html");
});

app.UseStaticFiles();
app.Run();
