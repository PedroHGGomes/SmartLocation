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

<img width="842" height="188" alt="image" src="https://github.com/user-attachments/assets/28e48991-6c29-4945-bc37-d0abe7c7f8f0" />


NotFound:

<img width="648" height="156" alt="image" src="https://github.com/user-attachments/assets/80f81c50-b93e-40a8-b13f-3cd9098cf90d" />


BadRequest:

<img width="641" height="167" alt="image" src="https://github.com/user-attachments/assets/03c7d9e9-b0ee-4cb4-8a4c-99f8f47a5f64" />


No Content:

<img width="597" height="299" alt="image" src="https://github.com/user-attachments/assets/2709f253-61ab-4efc-8ebe-d7742b075ff8" />


Created:

<img width="644" height="166" alt="image" src="https://github.com/user-attachments/assets/80f18f4b-1078-42de-abc6-5a30a979235d" />



 
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

<img width="1125" height="270" alt="image" src="https://github.com/user-attachments/assets/df9bbea3-ae5b-44a7-8e8d-c95decafb635" />

Swagger Motos:

<img width="1096" height="292" alt="image" src="https://github.com/user-attachments/assets/c090f6b3-ae80-42ac-8d1d-d4f5d6dd1565" />

Swagger Sensores:

<img width="1091" height="296" alt="image" src="https://github.com/user-attachments/assets/4074e22d-b327-48f0-8cf8-a6153b0db558" />

Swagger Usuarios:

![image](https://github.com/user-attachments/assets/1edb9520-8c43-4778-a694-f9826492e575)

Schemas (Com Paginação):

<img width="1094" height="290" alt="image" src="https://github.com/user-attachments/assets/7d69de1b-d19c-4b5e-9211-7de3e1ba1aa1" />

___
Payload:
___
Exemplos de Payloads

**Usuários**

**POST /api/usuarios**

{
  "nome": "Pedro Gomes",<br>
  "email": "pedro.gomes@mottu.com"<br>
}
___

**GET /api/usuarios?page=1&pageSize=2**

{
  "items": [<br>
    { "id": 1, "nome": "Pedro Gomes", "email": "pedro.gomes@mottu.com" },<br>
    { "id": 2, "nome": "Luiz Felipe Abreu", "email": "luiz.abreu@mottu.com" }<br>
  ],<br>
  "page": 1,<br>
  "pageSize": 2,<br>
  "totalCount": 5,<br>
  "totalPages": 3,<br>
  "links": {<br>
    "self": "http://localhost:5000/api/usuarios?page=1&pageSize=2",<br>
    "next": "http://localhost:5000/api/usuarios?page=2&pageSize=2"<br>
  }
}
___
**Motos**

**POST /api/motos**

{
  "placa": "ABC1234",<br>
  "renavam": "98765432100",<br>
  "modelo": "Honda CG",<br>
  "ano": 2022,<br>
  "quilometragem": 15000,<br>
  "status": "Disponivel",<br>
  "patioId": 1<br>
}

___
**GET /api/motos/1**

{
  "id": 1,<br>
  "placa": "ABC1234",<br>
  "renavam": "98765432100",<br>
  "modelo": "Honda CG",<br>
  "ano": 2022,<br>
  "quilometragem": 15000,<br>
  "status": "Disponivel",<br>
  "patioId": 1<br>
}

**Sensores**

**POST /api/sensores**

{
  "numero": 101<br>
}


**GET /api/sensores/search?numero=101**

[
  { "id": 1, "numero": 101 }
]

Endereços de Pátio

**POST /api/enderecopatios**

{
  "logradouro": "Rua das Flores",<br>
  "numero": "123",<br>
  "estado": "SP",<br>
  "cep": "01234000",<br>
  "patio": "Pátio Central"<br>
}


**GET /api/enderecopatios/1**

{
  "id": 1,<br>
  "logradouro": "Rua das Flores",<br>
  "numero": "123",<br>
  "estado": "SP",<br>
  "cep": "01234000",<br>
  "patio": "Pátio Central"<br>
}




