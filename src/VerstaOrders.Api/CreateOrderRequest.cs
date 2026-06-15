namespace VerstaOrders.Api;

public sealed record CreateOrderRequest(
    string SenderCity,
    string SenderAddress,
    string ReceiverCity,
    string ReceiverAddress,
    decimal WeightKg,
    DateOnly PickupDate)
{
    public bool IsValid() =>
        !string.IsNullOrWhiteSpace(SenderCity)
        && !string.IsNullOrWhiteSpace(SenderAddress)
        && !string.IsNullOrWhiteSpace(ReceiverCity)
        && !string.IsNullOrWhiteSpace(ReceiverAddress)
        && WeightKg > 0
        && PickupDate != default;
}
