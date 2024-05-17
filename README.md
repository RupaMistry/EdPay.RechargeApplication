
# **EdPay.Backend_TechnicalAssesement**

 ### <ins> **.NET Core Web API Project Description:-** <ins>

**I have used the below technology stack to build .NET Core Web APIs using Microservices & Clean architecture approach:-**
        
    a. Backend: .NET 8 Web API Core
    b. MSMQ: MassTransit: RabbitMQ for asynchronous inter service communication between UserAPI, RechargeAPI & PaymentAPI. 
    c. Polly Library: For handling communication between RechargeAPI & external HTTP service(in our case: PaymentAPI).
    d. Nuget NPM: For EdPay .Common & .Contracts library  code reusability.
    e. Docker: For MassTransit service local instance setup
    f. Database: SQL Server DB created using EF Core Code first migrations.
    g. Deployment: Couldnt setup entire deploymemt workflow due to issues with running a paid Parallel-Job for Azure DevOps Pipelines. 
                   But the .yaml configuration is setup under a private project in DevOps portal. 

I have added all the expected User story functionalities(Acceptance Criteria) except for Banking-Credit feature(as it was out of scope).

### <ins> **.NET Core Web API Project Setup Instructions:-** <ins>

**Please follow the steps to setup the project(s) and to check the key features.**

**A) EdPay.PaymentsApp.Api project in Services\Banking\EdPay.PaymentsApp folder**

1) Set "**EdPay.PaymentsApp.Api**" as startup project and then update the below in **appsettings.json** file:-

		a. Update "EdPayBankingAppDBConnection" configuration to point to your local SQLServer instance.

2) Open Nuget Package Manager console and select "EdPay.PaymentsApp.Infrastructure" as default project. Since migrations are already added into the project, we do not need to add any EF migrations. Please run both of the below command directly:

     	a. Add-Migration initialDatabaseCreation
	    b. update-database

    However, in case of any issues, you can delete the "**EdPay.PaymentsApp.Infrastructure\Migrations**" folder to get EF migrations file again and run the below commands in sequence as listed:
	
       a. Add-Migration initialDatabaseCreation
	   b. update-database

    Please open your SSMS IDE and validate if "**[EdPay.BankingAppDB]**" is created succesfully or not.  
 	
       a. Once confirmed, please open and execute the [spDebitTransaction.sql] script present under "**EdPay.PaymentsApp.Infrastructure\StoredProcedureScripts**"


**B) EdPay.RechargeApp.Api project in Services\Recharge\EdPay.RechargeApp**

1) Set "**EdPay.RechargeApp.Api**" as startup project and then update the below in **appsettings.json** file:-

		a. Update "EdPayRechargeAppDBConnection" configuration to point to your local SQLServer instance.

2) Open Nuget Package Manager console and select "EdPay.RechargeApp.Infrastructure" as default project. Since migrations are already added into the project, we do not need to add any EF migrations. Please run both of the below command directly:

     	a. Add-Migration initialDatabaseCreation
	    b. update-database

    However, in case of any issues, you can delete the "EdPay.RechargeApp.Infrastructure\Migrations" folder to get EF migrations file again and run the below commands in sequence as listed:
	
       a. Add-Migration initialDatabaseCreation
	   b. update-database

    Please open your SSMS IDE and validate if "**[EdPay.RechargeAppDB]**" is created succesfully or not.  

3)  Open **appsettings.json** file and update the PaymentApiClientUrl application setting to point to the url of hosted **EdPay.PaymentsApp.Api**


**C) EdPay.UserApp.Api project in Services\UserManagement\EdPay.UserApp**

1) Set "**EdPay.UserApp.Api**" as startup project and then update the below in **appsettings.json** file:-

		a. Update "EdPayUserAppDBConnection" configuration to point to your local SQLServer instance.

2) Open Nuget Package Manager console and select "EdPay.UserApp.Infrastructure" as default project. Since migrations are already added into the project, we do not need to add any EF migrations. Please run both of the below command directly:

     	a. Add-Migration initialDatabaseCreation
	    b. update-database

    However, in case of any issues, you can delete the "**EdPay.UserApp.Infrastructure\Migrations**" folder to get EF migrations file again and run the below commands in sequence as listed:
	
       a. Add-Migration initialDatabaseCreation
	   b. update-database

    Please open your SSMS IDE and validate if "**[EdPay.UserAppDB]**" is created succesfully or not.  

**D) Docker: MassTransit: RabbitMQ service and container setup**

1) Go to  "**src\Deployment**" project folder and verify the [docker-compose.yml] file:
   
	    a. Please make sure DockerDesktop is up and running on your local machine. 
     	b. Open Developer Powershell terminal and navigate to the Deployment folder location where docker-compose.yml file is present. 
     	c. Run below commands:
                i.  docker-compose up -d
                ii. docker ps

2) Open DockerDesktop tool and verify if "**rabbitmq**" service is up and running. If not, please follow above instructions.

3) Navigate to the "**rabbitmq**" management web tool by navigating to 'http://localhost:15672/' on browser. Login is "guest:guest". 

**E) Web API projects publish**

**Please follow the steps for running the Web API project(s) and to verify the key features.**

1) Configure <Multiple Starup Project> for **EdPay.PaymentsApp.Api, EdPay.RechargeApp.Api and EdPay.UserApp.Api** api projects. Please ensure all the projects are up and running on seperate hosts.

        a. You can use .API\SwaggerUI to test all methods of PaymentsApp API, RechargeApp API & UserApp API directly.

**F) Additional: Instructions for NUGET Pack of [EdPay.Common] class library.**

        a. EdPay Nuget packages are already correctly added to respective projects. 
           However if needed, please follow the setup instructions as specified in "[EdPay.Common\README.md]" file.

**G) Additional: Instructions for NUGET Pack of [EdPay.UserApp.Contracts] class library.**

        a. EdPay Nuget packages are already correctly added to respective projects. 
           However if needed, please follow the setup instructions as specified in "[EdPay.UserApp.Contracts\README.md]" file.

### <ins>  **EdPay Architecture Details:-**  <ins>

**Please check below high level applicatyion architecture diagrams.**

 ![EdPay Architecture](https://github.com/RupaMistry/EdPay.RechargeApplication/assets/10920962/fe2128c0-1c74-467b-bdf3-67f50c2bfb43)

 ![EdPay MSMQ Diagram](https://github.com/RupaMistry/EdPay.RechargeApplication/assets/10920962/6db313d5-5393-4a3b-838c-6358ec5b8d74)


