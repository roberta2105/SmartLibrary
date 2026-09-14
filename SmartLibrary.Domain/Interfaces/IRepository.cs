namespace SmartLibrary.Domain.Interfaces;

public interface IRepository <TEntity> where TEntity : class
{
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
}
