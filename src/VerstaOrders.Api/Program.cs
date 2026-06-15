using Microsoft.EntityFrameworkCore;
using VerstaOrders.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Orders") ?? "Data Source=orders.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();
    db.Database.EnsureCreated();
}

app.UseDefaultFiles();
app.UseStaticFiles();

var orders = app.MapGroup("/api/orders");

orders.MapGet("/", async (OrdersDbContext db) =>
    await db.Orders.OrderByDescending(order => order.Id).ToListAsync());

orders.MapGet("/{id:int}", async (int id, OrdersDbContext db) =>
    await db.Orders.FindAsync(id) is { } order
        ? Results.Ok(order)
        : Results.NotFound());

orders.MapPost("/", async (CreateOrderRequest request, OrdersDbContext db) =>
{
    if (!request.IsValid())
    {
        return Results.BadRequest("Все поля обязательны, вес должен быть больше нуля.");
    }

    var order = new DeliveryOrder
    {
        SenderCity = request.SenderCity.Trim(),
        SenderAddress = request.SenderAddress.Trim(),
        ReceiverCity = request.ReceiverCity.Trim(),
        ReceiverAddress = request.ReceiverAddress.Trim(),
        WeightKg = request.WeightKg,
        PickupDate = request.PickupDate
    };

    db.Orders.Add(order);
    await db.SaveChangesAsync();

    order.Number = $"ORD-{order.Id:D6}";
    await db.SaveChangesAsync();

    return Results.Created($"/api/orders/{order.Id}", order);
});

app.MapFallbackToFile("index.html");

app.Run();