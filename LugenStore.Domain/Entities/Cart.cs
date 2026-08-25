namespace LugenStore.Domain.Entities;

public class Cart
{
    public Guid Id { get; set; }
    public bool IsActive { get; set; }
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public List<CartItem> CartItens { get; set; } = new List<CartItem>();

}
