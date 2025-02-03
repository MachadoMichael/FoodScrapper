1. primeiro passo foi criar um docker-compose.yml com cshap, postgres e typescript separadamente mas na mesma rede.

2. depois iniciar o projeto usando o comando dotnet new webapi -n FoodScrapper

3. gosto de iniciar o projeto pelo banco de dados porque ja estrutura a base do sistema, entao usei os comandos:

````
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL --version 6.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.0
````
se atentar a versao do dotnet, porque comando padrao tras o latest.

4. colocar as strings de conexao no appsettings.json

5. 