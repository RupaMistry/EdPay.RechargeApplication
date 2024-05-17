namespace EdPay.PaymentsApp.Infrastructure.Repository
{
    /// <summary>
    /// IRepository for User's Beneficiary entity
    /// </summary>
    public class TransactionRepository(PaymentAppDBContext dbContext) : ITransactionRepository<TransactionHistory>
    { 
        private readonly PaymentAppDBContext _paymentAppDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

        /// <summary>
        /// Performs a debit transaction on User bank account system.
        /// </summary>
        /// <param name="debitTransactionRequest">The debit transaction request.</param>
        /// <returns><![CDATA[Task<DebitTransactionResponse>]]></returns>
        public async Task<DebitTransactionResponse> DebitAmountAsync(DebitTransactionRequest debitTransactionRequest)
        {
            try
            {
                // Perform the debit transaction using stored proc.
                var transactionID = await ExecuteDebitTransactionProc(debitTransactionRequest);

                // If newly inserted transactionID is not valid, return failure response.
                if (transactionID <= 0)
                    return new DebitTransactionResponse(TransactionStatusEnum.Failure, 0, transactionID, false, "Some error occured in Debit query execution");

                // else return success response.
                return await this.GetDebitTransactionResponse(transactionID);
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Performs a credit transaction on User bank account system.
        /// </summary>
        /// <param name="creditTransactionRequest">The credit transaction request.</param>
        /// <returns>A CreditTransactionResponse</returns>
        public CreditTransactionResponse CreditAmountAsync(CreditTransactionRequest creditTransactionRequest)
        {
            try
            {
                // NOTE: This functionality is not implemented as it was not part of the requirement scope.
                return new CreditTransactionResponse(TransactionStatusEnum.Success, creditTransactionRequest.UserID, 1, false, string.Empty);
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Executes Debit transaction procedure.
        /// </summary>
        /// <param name="debitTransactionRequest">The debit transaction request.</param>
        /// <returns><![CDATA[Task<int>]]></returns>
        private async Task<int> ExecuteDebitTransactionProc(DebitTransactionRequest debitTransactionRequest)
        {
            try
            {
                var userID = new SqlParameter
                {
                    ParameterName = "userID",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Value = debitTransactionRequest.UserID,
                };

                var beneficiaryID = new SqlParameter
                {
                    ParameterName = "beneficiaryID",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Value = debitTransactionRequest.BeneficiaryID,
                };

                var amount = new SqlParameter
                {
                    ParameterName = "amount",
                    SqlDbType = System.Data.SqlDbType.Decimal,
                    Value = debitTransactionRequest.Amount,
                };

                var currency = new SqlParameter
                {
                    ParameterName = "currency",
                    SqlDbType = System.Data.SqlDbType.VarChar,
                    Size = 3,
                    Value = debitTransactionRequest.AmountCurrency,
                };

                var serviceFee = new SqlParameter
                {
                    ParameterName = "serviceFee",
                    SqlDbType = System.Data.SqlDbType.Decimal,
                    Value = debitTransactionRequest.ServiceFee,
                };

                var transactionType = new SqlParameter
                {
                    ParameterName = "transactionType",
                    SqlDbType = System.Data.SqlDbType.NVarChar,
                    Size = 100,
                    Value = debitTransactionRequest.TransactionType,
                };

                var transactionID = new SqlParameter
                {
                    ParameterName = "transactionID",
                    SqlDbType = System.Data.SqlDbType.Int,
                    Direction = ParameterDirection.Output,
                };

                // Execute spDebitTransaction procedure.
                var spDebitTransactionQuery = $"EXEC [dbo].[spDebitTransaction] @userID, @beneficiaryID, @amount, @currency, @serviceFee, @transactionType, @transactionID OUTPUT; ";

                await _paymentAppDbContext.Database.ExecuteSqlRawAsync(spDebitTransactionQuery, userID, beneficiaryID, amount, currency, serviceFee, transactionType, transactionID);

                // Return OUT param @
                return Convert.ToInt32(transactionID?.Value);
            }
            catch (Exception) { throw; }
        }

        /// <summary>
        /// Returns DebitTransactionResponse.
        /// </summary>
        /// <param name="transactionID">The transaction ID.</param>
        /// <returns><![CDATA[Task<DebitTransactionResponse>]]></returns>
        private async Task<DebitTransactionResponse> GetDebitTransactionResponse(int transactionID)
        {
            var transactionDetails = await this._paymentAppDbContext.TransactionHistory.FirstOrDefaultAsync(t => t.ID == transactionID);

            if (transactionDetails == null)
                return new DebitTransactionResponse(TransactionStatusEnum.Failure, 0, 0, false, "Failed to retrieve TransactionHistory");
            else
                return new DebitTransactionResponse(
                   TransactionStatusEnum.Success, transactionDetails.UserID, transactionDetails.ID,
                   transactionDetails.IsSuccess, transactionDetails.ErrorMessage);
        }
    }
}