***Instructions for setting up "**[EdPay.RechargeAppDB]**" usign EF Core Code First Migration approach.**

1) Set "**EdPay.RechargeApp.Api**" as startup project and then update the below in **appsettings.json** file:-

		a. Update "EdPayRechargeAppDBConnection" configuration to point to your local SQLServer instance.

2) Open Nuget Package Manager console and select "EdPay.RechargeApp.Infrastructure" as default project. Since migrations are already added into the project, we do not need to add any EF migrations. Please run both of the below command directly:

     	a. Add-Migration initialDatabaseCreation
	    b. update-database

    However, in case of any issues, you can delete the "**EdPay.RechargeApp.Infrastructure\Migrations**" folder to get EF migrations file again and run the below commands in sequence as listed:
	
       a. Add-Migration initialDatabaseCreation
	   b. update-database

    Please open your SSMS IDE and validate if "**[EdPay.RechargeAppDB]**" is created succesfully or not.   