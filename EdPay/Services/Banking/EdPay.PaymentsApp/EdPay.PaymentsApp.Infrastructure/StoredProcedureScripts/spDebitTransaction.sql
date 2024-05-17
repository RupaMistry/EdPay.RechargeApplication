USE [EdPay.BankingAppDB]
GO
/****** Object:  StoredProcedure [dbo].[spDebitTransaction]    Script Date: 5/17/2024 12:04:18 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ======================================================================
-- Author:		Rupa Mistry
-- Create date: 12 May 2024
-- Description:	Procedure to handle debit transaction.
-- ======================================================================
CREATE OR ALTER PROCEDURE [dbo].[spDebitTransaction] 
	-- Add the parameters for the stored procedure here
	@userID int,
	@beneficiaryID int,
	@amount decimal,
	@currency varchar(3),
	@serviceFee decimal,
	@transactionType nvarchar(100),
	@transactionID int OUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 

	DECLARE @currentBalance decimal(10,2);
	DECLARE @totalAmount decimal(10,2);
	DECLARE @currentMonth int = MONTH(GETDATE());
	--DECLARE @isSucess int = 0;
	DECLARE @errorMessage nvarchar(max) = '';

	SET @totalAmount = @amount + @serviceFee;
	--SET @currentMonth = MONTH(GETDATE()); 

	-- Start a transaction to ensure data integrity
	BEGIN TRANSACTION;

	-- Get the current balance for the account
	SELECT @currentBalance = AvailableBalance
	FROM [dbo].[UserBankAccounts]
	WITH (HOLDLOCK) -- Lock the row to prevent conflicts
	WHERE [UserID] = @userID;
	 
	-- Check if sufficient funds are available
	IF @currentBalance < @totalAmount
	BEGIN
	 --RAISERROR ('Insufficient funds in user bank account.', 16, 1);
	 SET @errorMessage = 'Insufficient funds in user bank account.';

	 PRINT 'Insufficient funds Error.'

	 -- Roll back if insufficient balance;
	 ROLLBACK TRANSACTION;
	
	 -- Log the failed transaction record history
	 INSERT INTO [dbo].[TransactionHistory] 
           ([UserID], [BeneficiaryID], [IsCredit], [AmoutCurrency], [OpeningBalance], [ClosingBalance], 
		   [Amount], [ServiceFee], [CurrentMonth], [TransactionDate], [TransactionType], [IsSuccess], [ErrorMessage])
     VALUES
           (@userID, @beneficiaryID, 0, @currency, @currentBalance, @currentBalance,
		    @amount, @serviceFee, @currentMonth, GETDATE(), @transactionType, 0, @errorMessage);
	
	 SET @transactionID=SCOPE_IDENTITY(); 

	 Print 'Rollback complete.'
	 
	 RETURN;
	
	END;

	-- Requirement: The user's balance should be debited first before the top-up transaction is attempted.
	-- Update the account bank balance
	UPDATE [dbo].[UserBankAccounts]
	SET AvailableBalance = (AvailableBalance - @totalAmount)
	WHERE [UserID] = @userID;

	 
	-- Log the success transaction record history.
	INSERT INTO [dbo].[TransactionHistory] 
          ([UserID], [BeneficiaryID], [IsCredit], [AmoutCurrency], [OpeningBalance], [ClosingBalance], 
		   [Amount], [ServiceFee], [CurrentMonth], [TransactionDate], [TransactionType], [IsSuccess], [ErrorMessage])
     VALUES
           (@userID, @beneficiaryID, 0, @currency, @currentBalance, (@currentBalance - @totalAmount), 
		    @amount, @serviceFee, @currentMonth, GETDATE(), @transactionType , 1, '');
	
	SET @transactionID=SCOPE_IDENTITY(); 

	COMMIT TRANSACTION; -- Commit the transaction if successful
	 
	--RETURN  @transactionID;	

	 Print 'Transaction Success.'
END
