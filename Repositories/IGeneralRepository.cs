namespace Web_Social_network_BE.Repositories
{
    public interface IGeneralRepository<T, K>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(K key);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);
        Task DeleteAsync(K key);
    }
}
