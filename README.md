# Pedro Gomes - RM 553907
# Luiz Felipe Abreu - RM 555197
# Matheus Munuera - RM 557812


#SmartLocation

Descrição da Solução:

-	Descrição dos objetivos da solução idealizada:

 A solução envolve desenvolver um sistema de localização de cada moto em tempo real, com a utilização de tags bluetooth e mapa digital em tempo real.
Utilizaremos tanto o visual, com identificações numéricas de cada moto, e dentro desse objeto, colocaremos nossas tags para que seja possível realizar o rastreio.
Objetivos principais:
 •	Rastrear em tempo real cada moto dentro do pátio.
 •	Melhorar o controle de cada motocicleta estacionada.
 •	Melhorar a eficiência operacional.
 •	Aumentar a produtividade de cada funcionário à fim de reduzir custos.
 •	Redução do tempo de busca para encontrar uma motocicleta específica.
 •	Realizar uma organização visual dentro do pátio.

Exemplo visual:

![image](https://github.com/user-attachments/assets/9905d341-9297-4220-859d-600a1b3a1a62)

Exemplo 2D:

![image](https://github.com/user-attachments/assets/d49ee753-e343-452f-8b1a-5aafaf1479a8)

Arquitetura escolhida:

Optamos por uma arquitetura ASP.NET Core Web API com Entity Framework Core integrado ao Oracle Database.

Essa escolha foi feita porque:

 • O EF Core facilita o mapeamento objeto-relacional, simplificando a persistência dos dados.

 • Migrations permitem versionamento do banco e facilidade de evolução do schema.

 • A separação em Controllers, Models e DataContext segue boas práticas REST.

 • Swagger/OpenAPI fornece documentação interativa, simplificando testes e integração.

 • A arquitetura é escalável e permite futuras integrações com IoT (sensores) e aplicativos móveis.

Como executar a API:<br>


Clone este repositório:

git clone https://github.com/PedroHGGomes/SmartLocation.git
<br>
cd SmartLocation

___
Configure a connection string em appsettings.json:

"ConnectionStrings": {
  "DefaultConnection": "User Id=USUARIO;Password=SENHA;Data Source=localhost:1521/orcl"
}

___
Crie o banco de dados via migrations:
<br>
dotnet ef database update


Rode a aplicação:
<br>
dotnet run


Acesse o Swagger:
<br>
http://localhost:5254/swagger
<br>
________________________________________________________________________________________
Resumo do Código: <br>


- Implementar uma API Restful utilizando ASP.NET Core (Controllers ou Minimal API) 
 
 ![image](https://github.com/user-attachments/assets/a804a960-2deb-4f6e-a967-06bde5482cd0)

- Apresentar um CRUD pelo menos (GET (mais de 3 rotas e devidamente parametrizadas com QueryParams ou 
PathParams), POST, PUT, DELETE)
GET:

![image](https://github.com/user-attachments/assets/a7fba6b1-cb07-4e1d-9f1f-f84bbe45351e)

POST:

![image](https://github.com/user-attachments/assets/3e3404a5-2c4b-438b-954b-a11c12afc305)
![image](https://github.com/user-attachments/assets/f70b1646-45f8-46c2-8209-7ec54e1f1c0b)

PUT:

![image](https://github.com/user-attachments/assets/5b500666-f914-4b35-a748-5ee382d8aac2)
![image](https://github.com/user-attachments/assets/5a046c76-552d-4848-b812-3b6b80ee5226)

DELETE:

![image](https://github.com/user-attachments/assets/277c8e36-468f-47f9-9962-0178431689ba)



- Apresentar os retornos HTTP adequados para cada rota (ok, NotFound, BadRequest, NoContent, Created)
OK:

![image](https://github.com/user-attachments/assets/b57562c1-920d-4030-a370-4f83a001736d)

NotFound:

![image](https://github.com/user-attachments/assets/0788ba9b-faf3-4147-ab60-7af4f3457ce2)

BadRequest:

![image](https://github.com/user-attachments/assets/2dc66fcb-2051-4d36-ab08-ba42d73aecee)

No Content:

![image](https://github.com/user-attachments/assets/4317a720-f126-46cc-b5fe-48171a0e0530)

Created:

![image](https://github.com/user-attachments/assets/84414181-0a66-4362-889e-f7d0c9fae5e6)


 
- Integração do Banco de dados Oracle via EF Core, com utilização de migrations para criação das tabelas
 Alteração feita no .json:
 
![image](https://github.com/user-attachments/assets/c4773195-55ab-4183-85a7-2ffbf7ee4216)

Criação nos modelos:

![image](https://github.com/user-attachments/assets/8b48cad3-3f53-4332-9d42-f217e1c7ae3c)
![image](https://github.com/user-attachments/assets/70c1efb0-7c04-4c4c-9df7-5b3af584e462)

Conexão com Oracle no program.cs:

![image](https://github.com/user-attachments/assets/e980b1e1-63eb-4e66-91f2-1afdbb2d3f6e)



- Open API Implementada seguindo os padrões para documentação das API's com interface gráfica (Swagger, 
Redoc ou Scalar)

Swagger EnderecoPatios:

![image](https://github.com/user-attachments/assets/d1c6ffe0-b52c-4326-9dd6-cd27b997e329)

Swagger Motos:

![image](https://github.com/user-attachments/assets/cec61a4e-983a-48a7-b7d8-eb7b85db6052)

Swagger Sensores:

![image](https://github.com/user-attachments/assets/c8b52c3e-8612-4aad-877c-93045d7daa8b)

Swagger Usuarios:

![image](https://github.com/user-attachments/assets/1edb9520-8c43-4778-a694-f9826492e575)

Schemas:

![image](https://github.com/user-attachments/assets/6bcb34b9-a1a5-47f2-98b7-1cfaca2a691c)

Payload:

Exemplos de Payloads

Usuários

POST /api/usuarios

{
  "nome": "Pedro Gomes",
  "email": "pedro.gomes@mottu.com"
}


GET /api/usuarios?page=1&pageSize=2

{
  "items": [
    { "id": 1, "nome": "Pedro Gomes", "email": "pedro.gomes@mottu.com" },
    { "id": 2, "nome": "Luiz Felipe Abreu", "email": "luiz.abreu@mottu.com" }
  ],
  "page": 1,
  "pageSize": 2,
  "totalCount": 5,
  "totalPages": 3,
  "links": {
    "self": "http://localhost:5000/api/usuarios?page=1&pageSize=2",
    "next": "http://localhost:5000/api/usuarios?page=2&pageSize=2"
  }
}

Motos

POST /api/motos

{
  "placa": "ABC1234",
  "renavam": "98765432100",
  "modelo": "Honda CG",
  "ano": 2022,
  "quilometragem": 15000,
  "status": "Disponivel",
  "patioId": 1
}


GET /api/motos/1

{
  "id": 1,
  "placa": "ABC1234",
  "renavam": "98765432100",
  "modelo": "Honda CG",
  "ano": 2022,
  "quilometragem": 15000,
  "status": "Disponivel",
  "patioId": 1
}

Sensores

POST /api/sensores

{
  "numero": 101
}


GET /api/sensores/search?numero=101

[
  { "id": 1, "numero": 101 }
]

Endereços de Pátio

POST /api/enderecopatios

{
  "logradouro": "Rua das Flores",
  "numero": "123",
  "estado": "SP",
  "cep": "01234000",
  "patio": "Pátio Central"
}


GET /api/enderecopatios/1

{
  "id": 1,
  "logradouro": "Rua das Flores",
  "numero": "123",
  "estado": "SP",
  "cep": "01234000",
  "patio": "Pátio Central"
}




