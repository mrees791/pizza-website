namespace DataLibrary.Models
{
    public enum OrderStatus
    {
        OrderPlaced = 0,
        Prep = 1,
        Bake = 2,
        Box = 3,
        ReadyForPickup = 4,
        OutForDelivery = 5,
        Complete = 6,
        Cancelled = 7
    }
}