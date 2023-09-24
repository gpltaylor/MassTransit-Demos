
# MassTransit Console Apps
https://masstransit-project.com/usage/configuration.html#console-app

### Top Shelf
https://www.youtube.com/watch?v=FUa8RzWpJI0&ab_channel=CodeOpinion

### ASP.net Core
https://masstransit-project.com/usage/configuration.html#asp-net-core

### Tasks
https://docs.microsoft.com/en-us/dotnet/api/system.threading.tasks.task?view=net-5.0

## Start RabbitMQ
` docker run -d --hostname my-rabbit --name some-rabbit -p 8080:15672 -p 15672:15672 -p 5672:5672 rabbitmq:3-management
` docker stop some-rabbit && docker rm some-rabbit

## Setup Project and install required packages
` $ dotnet new console -o mtConsoleApp
` $ dotnet add package MassTransit --version 7.1.8
` $ dotnet add package MassTransit.Extensions.DependencyInjection --version 7.1.8
` $ dotnet add package Microsoft.Extensions.DependencyInjection --version 5.0.2
` $ dotnet add package MassTransit.AspNetCore --version 7.1.8
` $ dotnet add package MassTransit.RabbitMQ --version 7.1.8
` $ dotnet add reference ../EventContracts/EventContracts.csproj

## Enable logging for Console apps
` $ dotnet add package Microsoft.Extensions.Logging.Console
https://www.linkedin.com/pulse/how-build-net-core-31-console-application-built-in-injection-antu%C3%B1a/?articleId=6630094875025039361

## Topshelf
` dotnet add package Topshelf --version 4.3.0

Installing the services but raising permissions (needed on some systems)

` mtTopShelfConsumer.exe install
` mtTopShelfConsumer.exe uninstall
` Start-Process .\mtTopShelfConsumer.exe -verb runAs -Args "install"

# RabbitMQ Scheduling
For scheduing to work within RabbitMQ, a dedicated plugin must be enabled.
Below shows how to install this plugin.

https://github.com/rabbitmq/rabbitmq-delayed-message-exchange/releases
https://blog.rabbitmq.com/posts/2015/04/scheduling-messages-with-rabbitmq

` $ apt-get update 
` $ apt-get install wget
` $ wget https://github.com/rabbitmq/rabbitmq-delayed-message-exchange/releases
` $ rabbitmq-plugins enable rabbitmq_delayed_message_exchange
