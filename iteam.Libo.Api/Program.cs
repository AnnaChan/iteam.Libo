// Import necessary namespaces
using Microsoft.AspNetCore.ResponseCompression;
using iteam.Libo.Api.EndPoints;
using Microsoft.Extensions.Options;
using iteam.Libo.Api;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<LiboContext>();

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorOrigin", policy =>
    {
        policy.WithOrigins("https://localhost:7170","https://localhost:5190","https://localhost:5212")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

/*builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});*/

// Add other services
builder.Services.AddControllersWithViews();
builder.Services.AddRazorComponents();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

// Redirect HTTP to HTTPS if needed
app.UseHttpsRedirection();

// Apply the CORS policy
//app.UseCors("AllowAll");
app.UseCors("AllowBlazorOrigin");

// Serve static files and Blazor files
app.UseStaticFiles();
app.UseBlazorFrameworkFiles();

// Map endpoints for different resources
ArticleEndpoints.MapEndpoints(app);
ItemEndpoints.MapEndpoints(app);
UserEndpoints.MapEndpoints(app);
RoleEndpoints.MapEndpoints(app);

// Set up routing and map controllers
app.UseRouting();
app.MapControllers();

// Map fallback file for Blazor
app.MapFallbackToFile("index.html");

app.Run();
