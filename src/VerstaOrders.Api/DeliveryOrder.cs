namespace VerstaOrders.Api;

public sealed class DeliveryOrder
{
    public int Id { get; set; }
    public string Number { get; set; } = string.Empty;
    public string SenderCity { get; set; } = string.Empty;
    public string SenderAddress { get; set; } = string.Empty;
    public string ReceiverCity { get; set; } = string.Empty;
    public string ReceiverAddress { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public DateOnly PickupDate { get; set; }
}
