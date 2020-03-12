This project currently has an issue with DbContext scoping.

## TO RUN

1. Clone
2. If you need `dotnet ef`, run `dotnet tool restore`
3. Run `docker-compose up -d` to start the PostgreSQL server
4. Run the migrations, `dotnet ef database update --project SagaPostgres/SagaPostgres.csproj`
5. Run the application with `dotnet run --project SagaPostgres/SagaPostgres.csproj`
