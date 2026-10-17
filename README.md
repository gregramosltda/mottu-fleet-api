# 🏍️ Mottu Fleet API

API REST para gestão de frotas de motos elétricas — desenvolvida em **.NET 8** com **Clean Architecture**, **CQRS/MediatR**, autenticação **JWT**, versionamento de API e containerização com Docker.

> Projeto de portfólio profissional. Demonstra domínio de arquitetura limpa, padrões de projeto e boas práticas de engenharia back-end.

---

## 📌 Sumário

- [Visão Geral](#-visão-geral)
- [Stack Tecnológica](#-stack-tecnológica)
- [Arquitetura](#-arquitetura)
- [Estrutura de Pastas](#-estrutura-de-pastas)
- [Como Executar](#-como-executar)
- [Autenticação](#-autenticação)
- [Endpoints](#-endpoints)
- [Versionamento da API](#-versionamento-da-api)
- [Observabilidade](#-observabilidade)
- [Testes](#-testes)
- [Docker](#-docker)
- [Melhorias Futuras](#-melhorias-futuras)
- [Licença](#-licença)

---

## 🎯 Visão Geral

A **Mottu Fleet API** permite o cadastro, consulta e atualização de status de motos em uma frota, servindo de base para aplicações de gestão logística e mobilidade urbana.

**Principais recursos:**

- ✅ CRUD de motos com regras de negócio validadas (FluentValidation)
- ✅ Autenticação e autorização via **JWT Bearer**
- ✅ **CQRS** com MediatR (Commands e Queries separados)
- ✅ **Versionamento de API** (v1 e v2 simultâneas — v2 com paginação)
- ✅ Tratamento global de exceções via middleware
- ✅ **Rate limiting** (100 requisições/minuto por IP)
- ✅ **Health checks** (SQLite)
- ✅ Documentação interativa com **Swagger/OpenAPI**
- ✅ Logging estruturado com **Serilog** (console + arquivo com rotação diária)
- ✅ Persistência em **SQLite** via Entity Framework Core + Migrations
- ✅ Testes unitários e de integração

---

## 🧰 Stack Tecnológica

| Camada | Tecnologia |
|---|---|
| Runtime | .NET 8.0 (ASP.NET Core Web API) |
| Persistência | Entity Framework Core 8 + SQLite |
| Padrões | CQRS, MediatR, Repository, Options Pattern |
| Validação | FluentValidation 11 |
| Autenticação | JWT Bearer (`Microsoft.AspNetCore.Authentication.JwtBearer`) |
| Versionamento | `Asp.Versioning.Mvc` |
| Documentação | Swashbuckle (Swagger UI / OpenAPI) |
| Logging | Serilog (Console + File rolling diário) |
| Resiliência | Rate Limiting nativo (`System.Threading.RateLimiting`) |
| Health Checks | `AspNetCore.HealthChecks.Sqlite` |
| Testes | xUnit, Moq, FluentAssertions, `WebApplicationFactory` |
| Container | Docker multi-stage + Docker Compose |

---

## 🏗️ Arquitetura

Organização em camadas com separação clara de responsabilidades:

```
Controllers  →  MediatR (Commands/Queries)  →  Repositories  →  EF Core (SQLite)
     ↑                      ↑
 Validators           Middleware (exceções)
```

- **Controllers** — apenas orquestram a entrada HTTP e delegam ao MediatR.
- **Features** — regras de negócio isoladas por caso de uso (Command/Query + Handler).
- **Repositories** — abstração do acesso a dados (`IMotoRepository`).
- **Validators** — validações declarativas dos DTOs de entrada.
- **Middleware** — tratamento centralizado de exceções (`NotFoundException`, `ValidationException`).

---

## 📂 Estrutura de Pastas

```
mottu-fleet/
├── docker-compose.yml
├── Mottu.Fleet.Api/
│   ├── Config/            # JwtSettings (Options Pattern)
│   ├── Controllers/       # AuthController, MotosController, MotosV2Controller
│   ├── Data/              # AppDbContext
│   ├── Dtos/              # Request/Response DTOs
│   ├── Exceptions/        # Exceções de domínio
│   ├── Features/
│   │   └── Motos/
│   │       ├── Commands/  # CreateMoto, UpdateMotoStatus
│   │       └── Queries/   # GetAllMotos, GetMotoById
│   ├── Middleware/        # ExceptionHandlingMiddleware
│   ├── Migrations/        # EF Core Migrations
│   ├── Models/            # Moto, User
│   ├── Repositories/      # IMotoRepository, MotoRepository
│   ├── Services/          # IJwtService, JwtService
│   ├── Validators/        # FluentValidation
│   ├── Dockerfile
│   └── Program.cs
└── Mottu.Fleet.Tests/
    ├── Integration/       # Testes de endpoints
    └── Unit/
        ├── Handlers/      # Testes dos handlers CQRS
        └── Validators/    # Testes das validações
```

---

## 🚀 Como Executar

### Pré-requisitos

- [.NET SDK 8.0+](https://dotnet.microsoft.com/download/dotnet/8.0)
- (Opcional) Docker + Docker Compose

### Localmente

```bash
# Clonar
git clone https://github.com/gregramosltda/mottu-fleet-api.git
cd mottu-fleet-api/Mottu.Fleet.Api

# Restaurar dependências
dotnet restore

# Executar (aplica migrations automaticamente na inicialização)
dotnet run
```

A API sobe por padrão em `http://localhost:5272` e o Swagger em:

```
http://localhost:5272/swagger
```

### Banco de dados

O SQLite (`mottu.db`) é criado automaticamente no primeiro start — as **migrations são aplicadas em runtime** (`db.Database.Migrate()` em `Program.cs`). O arquivo **não é versionado** (ver `.gitignore`).

---

## 🔐 Autenticação

A API usa **JWT Bearer**. As rotas de motos exigem `[Authorize]`.

### Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "admin123"
}
```

**Resposta (200):**

```json
{
  "token": "eyJhbGciOiJIUzI1NiIs...",
  "username": "admin",
  "role": "Admin",
  "expiresAt": "2026-10-17T12:00:00Z"
}
```

### Usuários de demonstração

| Usuário | Senha | Role |
|---|---|---|
| `admin` | `admin123` | Admin |
| `operador` | `op123` | User |

> ⚠️ **Atenção:** credenciais hard-coded apenas para demonstração. Em produção, utilize um provedor de identidade real e secrets fora do código.

### Usando o token

```http
Authorization: Bearer <seu-token>
```

No Swagger UI, clique em **Authorize** e informe `Bearer <token>`.

---

## 🌐 Endpoints

### Auth

| Método | Rota | Descrição | Auth |
|---|---|---|---|
| `POST` | `/api/auth/login` | Autentica e retorna JWT | ❌ |

### Motos — v1

| Método | Rota | Descrição | Auth |
|---|---|---|---|
| `GET` | `/api/v1/motos` | Lista todas as motos | ✅ |
| `GET` | `/api/v1/motos/{id}` | Busca moto por ID | ✅ |
| `POST` | `/api/v1/motos` | Cadastra nova moto | ✅ |
| `PUT` | `/api/v1/motos/{id}/status` | Atualiza status da moto | ✅ |

### Motos — v2

| Método | Rota | Descrição | Auth |
|---|---|---|---|
| `GET` | `/api/v2/motos?page=1&size=10` | Lista motos com **paginação** | ✅ |

**Criar moto — corpo da requisição:**

```json
{
  "placa": "ABC1D23",
  "modelo": "Mottu Sport",
  "status": "disponivel",
  "quilometragem": 1200.5,
  "bateria": 87
}
```

**Atualizar status — corpo da requisição:**

```json
{ "status": "em_uso" }
```

**Resposta paginada (v2):**

```json
{
  "page": 1,
  "size": 10,
  "total": 42,
  "totalPages": 5,
  "data": [ /* ... */ ]
}
```

### Health Checks

| Rota | Descrição |
|---|---|
| `GET /health` | Status geral da aplicação |
| `GET /health/ready` | Verifica conectividade com o SQLite |

---

## 🔀 Versionamento da API

Duas versões coexistem e são expostas no Swagger:

- **v1** — contrato original (lista completa).
- **v2** — evolução com paginação (`page` / `size`), mantendo retrocompatibilidade.

O versionamento é feito por segmento de URL (`/api/v{version}/motos`) via `Asp.Versioning`.

---

## 📊 Observabilidade

- **Serilog**: logs no console e em `logs/mottu-YYYYMMDD.log` (rotação diária).
- **Health Checks**: `/health` e `/health/ready`.
- **Swagger UI**: exploração e teste interativo dos endpoints.
- **Rate Limiting**: janela fixa de 100 requisições/minuto por IP → `429 Too Many Requests`.

---

## 🧪 Testes

```bash
cd Mottu.Fleet.Tests
dotnet test
```

Cobertura:

- **Unitários** — handlers CQRS (`CreateMoto`, `UpdateMotoStatus`, `GetAllMotos`, `GetMotoById`) e validators (FluentValidation).
- **Integração** — endpoints HTTP via `WebApplicationFactory` + EF Core InMemory.

---

## 🐳 Docker

```bash
# Na raiz do projeto
docker compose up --build
```

- Build **multi-stage** (SDK → Runtime), imagem final enxuta baseada em `aspnet:8.0`.
- API exposta em `http://localhost:5272`.
- Banco persistido em volume nomeado (`mottu-data`).

---

## 🗺️ Melhorias Futuras

- [ ] Substituir usuários hard-coded por ASP.NET Core Identity
- [ ] Migrar de SQLite para PostgreSQL em produção
- [ ] Adicionar paginação também na v1 (deprecando-a formalmente)
- [ ] Mover segredos JWT para variáveis de ambiente / secret manager
- [ ] Pipeline CI/CD (GitHub Actions) com build, testes e análise estática
- [ ] Cobertura de testes com relatório (coverlet + Codecov)
- [ ] Cache distribuído (Redis) para consultas frequentes

---

## 📄 Licença

Este projeto está sob a licença **MIT**. Consulte o arquivo [LICENSE](LICENSE) para mais detalhes.

---

<p align="center">
  Desenvolvido por <strong>Gregory Ramos</strong> · .NET 8 · 2026
</p>
