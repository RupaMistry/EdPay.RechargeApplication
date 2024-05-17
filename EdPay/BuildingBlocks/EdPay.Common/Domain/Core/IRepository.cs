namespace EdPay.Common.Domain.Core
{
    /// <summary>
    /// Base IRepository.
    /// </summary>
    public interface IRepository<T> where T : Entity
    {
        /// <summary>
        /// Gets T entity details by ID.
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns>T entity</returns> 
        Task<T> GetAsync(int ID);

        /// <summary>
        /// Gets all T entities list.
        /// </summary> 
        /// <returns>List<T></returns> 
        Task<IReadOnlyList<T>> GetAllAsync();

        /// <summary>
        /// Creates a new T entity.
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>UserCreationEnum</returns> 
        Task<UserCreationEnum> CreateAsync(T entity);

        /// <summary>
        /// Updates a T entity.
        /// </summary>
        /// <param name="entity">entity</param>
        /// <returns>RowsAffected</returns> 
        Task<int> UpdateAsync(T entity);

        /// <summary>
        /// Deleted a T entity by ID.
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns>RowsAffected</returns> 
        Task<int> DeleteAsync(int ID);

        /// <summary>
        /// Checks if T entity exists or not.
        /// </summary>
        /// <param name="ID">ID</param>
        /// <returns>Boolean</returns> 
        Task<bool> Exists(int ID);
    }
}