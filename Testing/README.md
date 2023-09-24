```shell
dotnet new mtworker -n NewCo.Example.Green
cd NewCo.Example.Green
dotnet add package Microsoft.Extensions.DependencyInjection
```

```shell
cd ../
dotnet new  nunit -n NewCo.Example.Green.Tests
cd NewCo.Example.Green.Tests
dotnet add package Microsoft.Extensions.DependencyInjection
dotnet add reference ..\NewCo.Example.Green\NewCo.Example.Green.csproj
```

# Get Colour
```shell
cd ../NewCo.Example.Green
dotnet new mtconsumer -n SetColour --namespace NewCo.Example.Green.Consumers
```

# Colour Checker
```shell
cd ../NewCo.Example.Green
dotnet new mtconsumer -n ColourChecker --namespace NewCo.Example.Green.Consumers
```

# MassTransit.TestFramework - InMemoryTestFixture
```shell
dotnet add package MassTransit.TestFramework
```


# Round Two
```shell
dotnet new worker -o NewCo.Sales
dotnet new nunit -o .\NewCo.Sales.Tests
dotnet new mtconsumer -n GetLead --namespace NewCo.Sales
dotnet new mtconsumer -n AddSalesNote --namespace NewCo.Sales
```


## cd NewCo.Sales
```shell
dotnet build
```

### NewCo.Sales - Add MassTransit
Update program to register MT for in Memory

```c#
services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();
    x.AddConsumers(Assembly.GetEntryAssembly());
    x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));
});
```
### Load required Packages

```shell
dotnet add package MassTransit --version 8.0.15
dotnet add package MassTransit.RabbitMQ --version 8.0.15
```

* (not required?) dotnet add package MassTransit.AspNetCore --version 7.3.1
* (not required?) dotnet add package Microsoft.Extensions.DependencyInjection --version 7.0.0

## cd NewCo.Sales.Tests

```shell
dotnet add reference ../NewCo.Sales/NewCo.Sales.csproj
dotnet add package MassTransit.TestFramework --version 8.0.15
```

Using the code as a template below, we can test a consumer that returns a value. 
Using the MassTransit.Testing package, we can setup an in-memory queue and begin to set the Consumer quickly
https://github.com/MassTransit/MassTransit/blob/d289ae1ce58da7c5477d37558c5b8fce0fb57e5d/tests/MassTransit.Tests/Consumer_Specs.cs



Need to copy .vscode to allow debugging as the "template" didn't work.

Need to include DI to include the ServiceCollection.

``` 
<PackageReference Include="Microsoft.Extensions.Hosting" Version="6.0.0" />
```

## Running Tests
To run the tests from VS Code you need to update settings.json to identify where tests live.
You need a new project within the solution root.

```
"dotnet-test-explorer.testProjectPath": "${workspaceFolder}/Test.Example.Green.Tests/*.csproj",
```

## Send vs Publish
Within a basic Unit Test, Send will not work untill you register the EndPoint.

Publish will work out-of-the-box

# References
https://github.com/MassTransit/Sample-WebApplicationFactory/blob/master/tests/Sample.Tests/Sample.Tests.csproj
https://github.com/MassTransit/MassTransit/blob/d289ae1ce58da7c5477d37558c5b8fce0fb57e5d/tests/MassTransit.Tests/Consumer_Specs.cs

# TODO
Using InMemoryTestFixture, create a unit Test that returns data from the Consumer
Demo how Faults can be caught