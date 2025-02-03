1. primeiro passo foi criar um docker-compose.yml com cshap, postgres e typescript separadamente mas na mesma rede.

2. depois iniciar o projeto usando o comando dotnet new webapi -n FoodScrapper

3. gosto de iniciar o projeto pelo banco de dados porque ja estrutura a base do sistema, entao usei os comandos:

````
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 6.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.0
````
se atentar a versao do dotnet, porque comando padrao tras o latest.

4. colocar as strings de conexao no appsettings.json

5. criar model, context, service e controller para crud basico de food.

6. instalar o package para migrations
```
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 6.0.0
dotnet tool install --global dotnet-ef --version 6.0.0
```

7. criar primeira migration:
```
dotnet ef migrations add InitialCreate
```

8. aplicar a migration:
```
dotnet ef database update
```

x. instalar o package para ter o swagger funcionando
```
dotnet add package Swashbuckle.AspNetCore
```
