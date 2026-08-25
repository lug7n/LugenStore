namespace LugenStore.Domain.Entities;

public class CartItem
{
    public Guid Id { get; set; }
    public Guid GameId { get; set; }
    public Guid CartId { get; set; }
    public Game Game { get; set; } = null!;
    public Cart Cart { get; set; } = null!;
    public decimal PriceAtAddition { get; set; }
}
