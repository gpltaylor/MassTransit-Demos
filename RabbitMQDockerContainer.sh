docker run -d --hostname my-rabbit --name some-rabbit -p 8080:15672 -p 15672:15672 -p 5672:5672 rabbitmq:3-management

Top Shelf
https://www.youtube.com/watch?v=FUa8RzWpJI0&ab_channel=CodeOpinion

ASP.net Core
https://masstransit-project.com/usage/configuration.html#asp-net-core


$ dotnet new webapi -o AspnetCoreAPI
$ dotnet add package MassTransit.AspNetCore
$ dotnet add package MassTransit.RabbitMQ
$ dotnet new console -o mtConsoleApp