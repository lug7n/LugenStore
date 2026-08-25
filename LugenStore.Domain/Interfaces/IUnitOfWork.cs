namespace LugenStore.Infrastructure.Persistence;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
