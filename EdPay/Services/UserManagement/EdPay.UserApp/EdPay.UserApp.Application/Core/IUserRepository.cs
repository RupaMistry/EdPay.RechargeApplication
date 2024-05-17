namespace EdPay.UserApp.Application.Core
{
    /// <summary>
    /// IRepository for User entity
    /// </summary>
    public interface IUserRepository<T> : IRepository<T> where T : Entity
    { 
        /// <summary>
        /// Checks if user is verified or not.
        /// </summary>
        /// <param name="userID"></param> 
        /// <returns>Boolean</returns>
        Task<bool?> IsVerified(int userID);
    }
}