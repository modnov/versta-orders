using Microsoft.EntityFrameworkCore;

namespace VerstaOrders.Api;

public sealed class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContext(options)
{
    public DbSet<DeliveryOrder> Orders => Set<DeliveryOrder>();
}
