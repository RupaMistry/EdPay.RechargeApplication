namespace EdPay.RechargeApp.Api.ExternalServiceClients
{
    /// <summary>
    /// Payment API Client represents external HTTP service(PaymentsAPI microservice)
    /// </summary>
    /// <param name="httpClient">The http client.</param>
    /// <param name="logger">The logger.</param>
    public class PaymentApiClient(HttpClient httpClient, ILogger<PaymentApiClient> logger)
    {
        private readonly HttpClient _httpClient = httpClient;

        private readonly ILogger<PaymentApiClient> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Returns current available balance details for given userID from PaymentsAPI
        /// </summary>
        /// <param name="userID">The user ID.</param>
        /// <returns><![CDATA[Task<UserBankingDetails>]]></returns>
        public async Task<UserBankingDetails> GetUserBankBalanceAsync(int userID)
        {
            var userBankingDetails = new UserBankingDetails(0, 0);
            try
            {
                // Create a GET request with userID value and parse response.
                userBankingDetails = await _httpClient.GetFromJsonAsync<UserBankingDetails>
                    ($"{ServiceClientApiURL.Payment_GetBalance}?userID={userID}");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }

            return userBankingDetails;
        }

        /// <summary>
        /// Performs a debit transaction on User bank account system
        /// </summary>
        /// <param name="rechargePayment">The recharge payment.</param>
        /// <returns><![CDATA[Task<DebitTransactionResponse>]]></returns>
        public async Task<DebitTransactionResponse> DebitAmountAsync(RechargePayment rechargePayment)
        {
            try
            {
                // Create a POST request with rechargePayment value and parse response.
                var debitResponse = await _httpClient.PostAsJsonAsync<RechargePayment>
                    ($"{ServiceClientApiURL.Payment_DebitTransaction}", rechargePayment);

                // Ensure we get OK response from client api.
                debitResponse.EnsureSuccessStatusCode();

                var responseDetails = await debitResponse.Content.ReadFromJsonAsync<DebitTransactionResponse>();

                return responseDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}