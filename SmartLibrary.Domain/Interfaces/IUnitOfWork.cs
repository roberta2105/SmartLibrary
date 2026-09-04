namespace SmartLibrary.Domain.Interfaces;

public interface IUnitOfWork
{
    Task CommitAsync();
}
