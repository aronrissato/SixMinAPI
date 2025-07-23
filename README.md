# SixMinAPI
.NET 6 Minimal API
- NuGet packages to install
```
dotnet add package Microsoft.EntityFrameworkCore --version 6.0.5
dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.5
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.5
dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection --version 11.0.0
```
- Using user secrets (UserSecretsId)
```
dotnet user-secrect init

dotnet user-secrets set "UserId" "sa"
dotnet user-secrets set "Password" "Aron1234!"

Obs: In Program.cs it will be like this:
sqlConBuilder.ConnectionString = builder.Configuration.GetConnectionString("SQLDbConnection");
sqlConBuilder.UserID = builder.Configuration["UserId"];
sqlConBuilder.Password = builder.Configuration["Password"];

```

Docker container (SQLServer)
- download docker desktop
To start a new container reading docker-compose.yaml
```
docker-compose up -d
```
Check if the container is running:
```
docker ps 
```

EntityFramework
```
dotnet ef migrations add initialMigration
dotnet ef database update
```

#
##
###