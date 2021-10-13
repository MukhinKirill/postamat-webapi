namespace PostamatService.Data.Models
{
    public enum OrderStatus
    {
        Registered = 1,
        InStock = 2,
        InCourier = 3,
        DeliveredToPostamat = 4,
        DeliveredToRecipient = 5,
        Canceled = 6,
    }
}