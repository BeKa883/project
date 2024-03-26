using System.ComponentModel.DataAnnotations.Schema;

namespace Ass03.Models;

public class ProductCart
{
    public int ProductCartId { get; set; }
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    // Optional: Store price at the time of adding to the cart if it can change over time
    public int? PriceAtTimeOfAdding { get; set; }
}
