***Instructions for setting up "**[EdPay.UserAppDB]**" usign EF Core Code First Migration approach.**

1) Set "**EdPay.UserApp.Api**" as startup project and then update the below in **appsettings.json** file:-

		a. Update "EdPayUserAppDBConnection" configuration to point to your local SQLServer instance.

2) Open Nuget Package Manager console and select "EdPay.UserApp.Infrastructure" as default project. Since migrations are already added into the project, we do not need to add any EF migrations. Please run both of the below command directly:

     	a. Add-Migration initialDatabaseCreation
	    b. update-database

    However, in case of any issues, you can delete the "**EdPay.UserApp.Infrastructure\Migrations**" folder to get EF migrations file again and run the below commands in sequence as listed:
	
       a. Add-Migration initialDatabaseCreation
	   b. update-database

    Please open your SSMS IDE and validate if "**[EdPay.UserAppDB]**" is created succesfully or not.   