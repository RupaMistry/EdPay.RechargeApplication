namespace EdPay.UserApp.Api.ExtensionMethods
{
    /// <summary>
    /// Extension methods for Api.ViewModels.
    /// </summary>
    public static class ViewModelMapperExtensions
    {
        /// <summary>
        /// Maps and return Beneficiary.
        /// </summary>
        /// <param name="model">The view model.</param>
        /// <returns>Beneficiary</returns>
        public static Beneficiary AsEntity(this BeneficiaryModel model)
        {
            return new Beneficiary()
                { UserID = model.UserID, NickName = model.NickName, PhoneNumber = model.PhoneNumber };
        }

        /// <summary>
        /// Maps and return User.
        /// </summary>
        /// <param name="model">The view model.</param>
        /// <returns>User</returns>
        public static User AsEntity(this UserModel model)
        {
            return new User()
            {
                UserName = model.UserName,
                FullName = model.FullName,
                PhoneNumber = model.PhoneNumber,
                EmailAddress = model.EmailAddress,
                Nationality = model.Nationality,
                IsVerified = model.IsVerified
            };
        }
    }
}