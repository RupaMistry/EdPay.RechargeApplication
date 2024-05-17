### <ins> ﻿***Instructions for setting up "**[EdPay.BankingAppDB]**" usign EF Core Code First Migration approach.**

1) Set "**EdPay.PaymentsApp.Api**" as startup project and then update the below in **appsettings.json** file:-

		a. Update "EdPayBankingAppDBConnection" configuration to point to your local SQLServer instance.

2) Open Nuget Package Manager console and select "EdPay.PaymentsApp.Infrastructure" as default project. Since migrations are already added into the project, we do not need to add any EF migrations. Please run both of the below command directly:

     	a. Add-Migration initialDatabaseCreation
	    b. update-database

    However, in case of any issues, you can delete the "**EdPay.PaymentsApp.Infrastructure\Migrations**" folder to get EF migrations file again and run the below commands in sequence as listed:
	
       a. Add-Migration initialDatabaseCreation
	   b. update-database

    Please open your SSMS IDE and validate if "**[EdPay.BankingAppDB]**" is created succesfully or not.  
 	
       a. Once confirmed, please open and execute the spDebitTransaction.sql script present under "**EdPay.PaymentsApp.Infrastructure\StoredProcedureScripts**"
