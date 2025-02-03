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

curl -X POST http://localhost:4000/api/components \
-H "Content-Type: application/json" \
-d '{
    "name": "Vitamin C",
    "units": "mg",
    "valuePer100g": 50.0,
    "standardDeviation": "5",
    "minValue": 45.0,
    "maxValue": 55.0,
    "numberOfDataUsed": 10,
    "references": "Nutritional Database",
    "dataType": "Calculated",
    "foodId": 1 // Substitua pelo ID do Food que você criou
}'
