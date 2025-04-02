# Loja - Sistema de Gestão de Vendas

Sistema de API RESTful para gerenciamento de vendas, desenvolvido seguindo princípios de Domain-Driven Design (DDD) e com uso do padrão External Identities para referenciação de entidades de outros domínios.

## Sumário

- [Visão Geral](#visão-geral)
- [Arquitetura e Organização](#arquitetura-e-organização)
- [Regras de Negócio](#regras-de-negócio)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Configuração e Execução](#configuração-e-execução)
- [Endpoints da API](#endpoints-da-api)
- [Publicação de Eventos](#publicação-de-eventos)
- [Testes](#testes)
- [Decisões Técnicas](#decisões-técnicas)

## Visão Geral

A Loja.API é uma API RESTful que gerencia registros de vendas completos, incluindo:
- Registro de vendas com número, data e cliente
- Gerenciamento de itens de venda com quantidades e descontos
- Registro de filiais e produtos
- Cancelamento de vendas e itens
- Publicação de eventos de negócio

## Arquitetura e Organização

O projeto segue uma arquitetura limpa (Clean Architecture) com uma divisão clara de responsabilidades:

- **Camada de Domínio**: Contém as entidades de negócio, value objects, regras de negócio e interfaces de repositório
- **Camada de Aplicação**: Contém os serviços de aplicação, DTOs e lógica de aplicação
- **Camada de Infraestrutura**: Contém as implementações de repositório, contexto de banco de dados e publicação de eventos
- **Camada de API**: Contém os controladores REST e a configuração da aplicação

## Regras de Negócio

- Compras acima de 4 itens idênticos têm 10% de desconto
- Compras entre 10 e 20 itens idênticos têm 20% de desconto
- Não é possível vender acima de 20 itens idênticos
- Compras abaixo de 4 itens não podem ter desconto

## Tecnologias Utilizadas

- ASP.NET Core 7.0
- Entity Framework Core 7.0
- SQL Server
- AutoMapper
- xUnit para testes
- Swagger para documentação da API

## Estrutura do Projeto

```
Loja/
├── Loja.Domain/
│   ├── Entities/
│   ├── ValueObjects/
│   ├── Events/
│   └── Interfaces/
├── Loja.Application/
│   ├── DTOs/
│   ├── Services/
│   ├── Interfaces/
│   └── AutoMapperConfig/
├── Loja.Infrastructure/
│   ├── Data/
│   │   ├── Repositories/
│   │   └── EntityConfigurations/
│   └── EventPublisher/
├── Loja.API/
│   ├── Controllers/
│   └── Program.cs
└── Loja.Tests/
    ├── Unit/
    └── Integration/
```

## Configuração e Execução

### Pré-requisitos

- .NET 7.0 SDK
- SQL Server (ou SQL Server Express)
- Visual Studio 2022 ou Visual Studio Code

### Configuração do Banco de Dados

1. Abra o arquivo `appsettings.json` e configure a string de conexão:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=SEU_SERVIDOR;Database=DeveloperStoreDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
}
```

2. Abra o Package Manager Console e execute os seguintes comandos:

```
Add-Migration InitialCreate -Project DeveloperStore.Infrastructure -StartupProject DeveloperStore.API
Update-Database -Project DeveloperStore.Infrastructure -StartupProject DeveloperStore.API
```

### Executando o Projeto

1. Clone o repositório:

```bash
git clone https://github.com/seu-usuario/developer-store.git
cd developer-store
```

2. Restaure os pacotes e compile o projeto:

```bash
dotnet restore
dotnet build
```

3. Execute a API:

```bash
cd DeveloperStore.API
dotnet run
```

4. Acesse a documentação Swagger em https://localhost:5001/swagger

## Endpoints da API

### Sales

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | /api/sales | Retorna todas as vendas |
| GET | /api/sales/{id} | Retorna uma venda específica com detalhes |
| GET | /api/sales/customer/{customerId} | Retorna vendas de um cliente específico |
| GET | /api/sales/branch/{branchId} | Retorna vendas de uma filial específica |
| GET | /api/sales/date-range?startDate={start}&endDate={end} | Retorna vendas em um intervalo de datas |
| POST | /api/sales | Cria uma nova venda |
| PUT | /api/sales | Atualiza uma venda existente |
| POST | /api/sales/cancel | Cancela uma venda |
| POST | /api/sales/cancel-item | Cancela um item de uma venda |
| DELETE | /api/sales/{id} | Remove uma venda (cancelamento) |

### Exemplo de Requisição para Criar uma Venda

```json
{
  "customerExternalId": "CUST123",
  "branchExternalId": "BR001",
  "items": [
    {
      "productExternalId": "PROD001",
      "quantity": 5,
      "unitPrice": 1500.00
    },
    {
      "productExternalId": "PROD002",
      "quantity": 2,
      "unitPrice": 800.00
    }
  ]
}
```

## Publicação de Eventos

O sistema está preparado para publicar os seguintes eventos:

- **SaleCreatedEvent**: Quando uma nova venda é criada
- **SaleModifiedEvent**: Quando uma venda é modificada
- **SaleCancelledEvent**: Quando uma venda é cancelada
- **ItemCancelledEvent**: Quando um item de venda é cancelado

Nesta implementação, os eventos são registrados no log da aplicação, mas a estrutura está preparada para integração com sistemas de mensageria como RabbitMQ, Kafka ou Azure Service Bus.

## Testes

O projeto inclui testes unitários e de integração. Para executá-los:

```bash
cd DeveloperStore.Tests
dotnet test
```

Os testes cobrem:
- Regras de negócio do domínio
- Funcionamento dos value objects
- Integração com o banco de dados

## Decisões Técnicas

### Padrão External Identity

Foi implementado o padrão "External Identities" para as entidades de outros domínios (Customer, Branch, Product). Cada entidade possui um identificador externo (ExternalId) e campos denormalizados com as informações básicas, permitindo referências a entidades gerenciadas por outros microsserviços.

### Value Objects

Foi implementado o Value Object Money para encapsular valores monetários, garantindo a integridade dos valores e facilitando operações com valores monetários.

### Eventos de Domínio

Os eventos de domínio foram implementados para permitir comunicação assíncrona entre sistemas, seguindo os princípios de arquitetura orientada a eventos.

### Entity Framework Core

O Entity Framework Core foi configurado com mapeamento explícito para todas as entidades e value objects, garantindo que o esquema do banco de dados reflita corretamente o modelo de domínio.
