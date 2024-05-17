namespace EdPay.Common.Domain
{
    /// <summary>
    /// Represents Currency constants
    /// </summary>
    public class Currency
    {
        public const string UAE = "AED";
        public const string INDIA = "INR";
    }
 
    /// <summary>
    /// Represents User creation status enum
    /// </summary>
    public enum UserCreationEnum
    {
        UserNameExists = 1,
        EmailIDExists = 2,
        PhoneNumberExists = 3,
        InvalidUserError = 4,
        BeneficiaryLimitExceeded = 5,
        InvalidUserID = 6,
        UserCreated = 7
    }

    /// <summary>
    /// Represents Currency constants
    /// </summary>
    public class MassTransitConstants
    {
        public const int RetryCount = 3;
        public const int RetryTimeInSeconds = 4;
    }
}
