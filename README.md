# ICMarkets test assignment
This is a web API project that stores blockchain snapshots for some networks from external [BlockCypher API](https://www.blockcypher.com/dev/bitcoin/#blockchain-api) into internal SQLite database and exposes endpoints to read snapshots history. The solution is built on [.net 10](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-10/overview). It was chosen not only because it's the latest release but also due to its [long-term support (LTS)](https://dotnet.microsoft.com/platform/support/policy/dotnet-core).

## Build & Run
Application is built into docker image and run as a container using standard commands:
```
docker build -t icmarkets-test .
docker run -p 8080:8080 icmarkets-test
```
> [!NOTE]  
> Application in docker container is configured for `Development` environment. In general docker images are built with production configuration. However it was done here only for testing reasons in order to include Swagger.

After that application will be available [locally on standard 8080 port](http://localhost:8080/):

![Swagger UI](images/swagger-ui.png?raw=true)

## Tests
All the available tests can be run as standard:
```
dotnet test
```
The results should be something like:

![Test Results](images/tests.png?raw=true)

## API Endpoints

- `GET /api/v1/networks`

Get list of supported networks in alphabetical order with paging. According to the task, there are five of them: btc-main, btc-test3, eth-main, dash-main, ltc-main.

- `GET /api/v1/networks/{networkId}`

Get single supported network.

- `POST /api/v1/networks/{networkId}/snapshots`

Take blockchain snapshot for specified network from BlockCypher public API and stores it in internal database.

- `GET /api/v1/networks/{networkId}/snapshots`

Get list of blockchain snapshots for specified network in reverse chronological order with paging.

- `GET /api/v1/networks/{networkId}/snapshots/{snapshotId}`

Get single blockchain snapshot.


## Key points

- Web API is build on top of MVC framework with some additional features like JSON serialization, auto validation, global exception handling, basic CORS policy and healthcheck endpoint. Also it has swagger ui with detailed annotations.

- Dependency injection is implemented with native .net DI container with auto validation on application start.

- There was a huge temptation to use libraries like [MediatR](https://github.com/LuckyPennySoftware/MediatR) and [AutoMapper](https://github.com/LuckyPennySoftware/AutoMapper), however they cannot be freely used for commercial development anymore. That's why new [Mapperly](https://github.com/riok/mapperly) library was used. It generates mappers with .net source generators, so it doesn't add runtime overhead with reflection.

- SQLite is used for database under EF Core. Project has migrations for further development needs. Initial migration is applied on application start.

- [Refit](https://github.com/reactiveui/refit) library was used for describing REST API client. It significantly reduces amount of boilerplate code related to outgoing http calls. [Microsoft.Extensions.Http.Resilience](https://github.com/dotnet/extensions/blob/main/src/Libraries/Microsoft.Extensions.Http.Resilience/README.md) library that is built on the [Polly](https://github.com/App-vNext/Polly) was used for resilience needs to describe timeout and retry policies and respect rate limits.

- [Serilog](https://github.com/serilog/serilog) is used for structured logging as a gold standard. All the requests and messages are logged to console output.

- Application uses some [OpenTelemetry](https://github.com/open-telemetry/opentelemetry-dotnet) libraries to collect runtime, asp.net core, ef core and http client metrics. All of them are available in prometheus compatible format on `/metrics` endpoint.

- Project uses [xUnit](https://github.com/xunit/xunit), [Moq](https://github.com/devlooped/moq) and [Shouldly](https://github.com/shouldly/shouldly) libraries for testing needs. Integration and functional tests use SQLite databse in-memory mode.


## Points for improvement

- Move from MVC framework to [Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis). It's modern approach for microservices that can nicely suit for CQRS and vertical slices structure.

- Move networks from configuration (`appsettings.json`) to the database. It will allow to modify supported networks set without re-configuration.

- [Central Package Management](https://learn.microsoft.com/en-us/nuget/consume-packages/central-package-management). It allows to manage NuGet dependencies versions globally and keep them consistent throughout the solution.

- Add API gateway project like was mentioned as optional in the original task. It can be built with either a reverse-proxy like [YARP](https://github.com/dotnet/yarp), or out-of-the-box [Ocelot](https://github.com/threemammals/ocelot) gateway.