### <ins> **Docker: Instructions for setting up and deploying RabbitMQ instance on Docker server.**

1. Please make sure DockerDesktop is up and running on local machine

2. Open cmd or Powershell and navigate to the Deployment folder location where [docker-compose.yml] file is present.

3. Run below commands

		a. docker-compose up -d
		b. docker ps 

5. Open DockerDesktop tool and verify if "**rabbitmq**" service is up and running. If not, please follow above instructions.

6. Navigate to the "**rabbitmq**" management web tool by navigating to locahost on browser. Login is "guest:guest". 
