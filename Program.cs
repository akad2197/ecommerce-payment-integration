var builder = WebApplication.CreateBuilder(args);

// Balance Management API servisleri
builder.Services.AddScoped<IBalanceOrderService, BalanceOrderService>();
builder.Services.AddHttpClient<IBalanceOrderService, BalanceOrderService>(client =>
{
    var baseUrl = builder.Configuration["BalanceApi:BaseUrl"] 
        ?? throw new InvalidOperationException("BalanceApi:BaseUrl configuration is missing");
    client.BaseAddress = new Uri(baseUrl);
});

// Database configuration
builder.Services.AddDbContext<OrderDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// Repository registration
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Swagger servisleri
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Controller
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ECommerce API v1");
        c.RoutePrefix = string.Empty;
    });
}

// app.UseHttpsRedirection(); // Docker ortamında kapalı kalabilir
app.UseAuthorization();
app.MapControllers();

// EF Core Migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
    db.Database.Migrate();
}

app.Run();
