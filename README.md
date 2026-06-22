# 🎵 MusicMatch

> Plataforma que conecta **artistas** e **contratantes de eventos** com base em compatibilidade de agenda, estilo musical, localização e equipamento.

---

## 🧱 Stack

### Backend
| Tecnologia | Função |
|---|---|
| .NET 8 + ASP.NET Core | Plataforma e camada HTTP |
| Entity Framework Core 8 | ORM + Migrations |
| PostgreSQL | Banco de dados relacional |
| MediatR | CQRS (Commands e Queries) |
| FluentValidation | Validação de entrada |
| RabbitMQ + MailKit | Mensageria e notificações por e-mail |
| Google OAuth + JWT | Autenticação e autorização |
| Swagger (OpenAPI) | Documentação interativa da API |

### Frontend
| Tecnologia | Função |
|---|---|
| React 18 + TypeScript | Interface de usuário |
| Tailwind CSS | Estilização utilitária |
| Vite | Build e dev server |
| Axios | Cliente HTTP |

### Infraestrutura
| Tecnologia | Função |
|---|---|
| Docker + Docker Compose | PostgreSQL e RabbitMQ em container |
| GitHub Actions | CI/CD automático a cada push |

---

## 📂 Estrutura do Projeto

```
musicmatch/
│
├── MusicMatch.API/                  # Apresentação — Controllers, Program.cs, configurações
├── MusicMatch.Application/          # Casos de uso — Commands, Queries, Handlers, Validators, DTOs
├── MusicMatch.Domain/               # Núcleo — Entidades, Enums, Interfaces
├── MusicMatch.Infrastructure/       # I/O — EF Core, Repositórios, RabbitMQ, Migrations
├── MusicMatch.Authentication/       # JWT + Google OAuth
│
├── tests/
│   ├── MusicMatch.Tests.Unit/       # Testes unitários (xUnit + FluentAssertions)
│   └── MusicMatch.Tests.Integration/# Testes de integração (WebApplicationFactory)
│
├── musicmatch-frontend/             # SPA React + TypeScript + Tailwind
├── docker-compose.yml               # PostgreSQL + RabbitMQ
└── .github/workflows/ci.yml         # Pipeline CI
```

### Responsabilidades por camada

| Camada | Responsabilidade |
|---|---|
| API | Receber requisições HTTP, retornar respostas |
| Application | Orquestrar regras de negócio via MediatR |
| Domain | Entidades e regras de negócio puras |
| Infrastructure | Banco de dados, RabbitMQ, repositórios |
| Authentication | JWT + Google OAuth |
| Tests | Unitários e integração |

---

## ⚙️ Pré-requisitos

- [.NET SDK 8](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (com WSL2 no Windows)
- Git

```bash
dotnet --version   # 8.0.x
node --version     # v18+ ou v20.x
docker --version   # 20+
```

---

## ▶️ Como Rodar o Projeto

### 1. Clonar o repositório

```bash
git clone https://github.com/josefelipesouza/musicmatch.git
cd musicmatch
```

---

### 2. Configurar o `appsettings.json`

Copie o arquivo de exemplo:

```bash
cp MusicMatch.API/appsettings.Example.json MusicMatch.API/appsettings.json
```

Edite `MusicMatch.API/appsettings.json` e preencha:

| Chave | Descrição |
|---|---|
| `ConnectionStrings.DefaultConnection` | String de conexão com o PostgreSQL |
| `Authentication.Google.ClientId` | Client ID do Google Cloud Console |
| `Authentication.Google.ClientSecret` | Client Secret do Google Cloud Console |
| `Authentication.Jwt.Secret` | Chave secreta longa e aleatória para assinar tokens JWT |
| `Smtp` | Credenciais de e-mail para envio de notificações |


---

### 3. Subir a infraestrutura (PostgreSQL + RabbitMQ)

```bash
docker-compose up -d
```

| Serviço | URL |
|---|---|
| PostgreSQL | `localhost:5432` |
| RabbitMQ | `localhost:5672` |
| RabbitMQ UI | [http://localhost:15672](http://localhost:15672) — `guest / guest` |

---

### 4. Iniciar o Backend

```bash
dotnet restore
dotnet run --project MusicMatch.API
```

| Recurso | URL |
|---|---|
| API | [http://localhost:5216](http://localhost:5216) |
| Swagger | [http://localhost:5216/swagger](http://localhost:5216/swagger) |

> As migrations do banco de dados são aplicadas automaticamente na inicialização.

---

### 5. Iniciar o Frontend

Abra um **novo terminal**:

```bash
cd musicmatch-frontend
npm install
npm run dev
```

| Recurso | URL |
|---|---|
| Frontend | [http://localhost:5173](http://localhost:5173) |

---

## 🔐 Autenticação

O login é feito via **Google OAuth**:

| Endpoint | Descrição |
|---|---|
| `GET /api/auth/google` | Inicia o fluxo OAuth com o Google |
| `GET /api/auth/google/callback` | Callback do Google — gera o JWT e redireciona para o frontend |

No primeiro acesso, o usuário é direcionado para completar o cadastro como **artista** ou **contratante**.

---

## 🗄️ Banco de Dados e Migrations

```bash
# Criar nova migration
dotnet ef migrations add NomeDaMigration \
  --project MusicMatch.Infrastructure \
  --startup-project MusicMatch.API

# Aplicar migrations manualmente
dotnet ef database update \
  --project MusicMatch.Infrastructure \
  --startup-project MusicMatch.API
```

Migrations ficam em `MusicMatch.Infrastructure/Migrations/`.

---

## 📦 Principais Endpoints da API

### Artistas

| Método | Endpoint | Descrição |
|---|---|---|
| `POST` | `/api/artistas` | Cadastrar artista |
| `GET` | `/api/artistas/{id}` | Buscar artista por ID |
| `PUT` | `/api/artistas/{id}` | Atualizar perfil |
| `GET` | `/api/artistas/buscar` | Match com evento (Haversine + filtros) |
| `POST` | `/api/artistas/agenda` | Criar agenda de disponibilidade |
| `GET` | `/api/artistas/{id}/agendas` | Listar agendas |
| `DELETE` | `/api/artistas/agenda/{agendaId}` | Cancelar agenda |

### Contratantes

| Método | Endpoint | Descrição |
|---|---|---|
| `POST` | `/api/contratantes` | Cadastrar contratante |
| `GET` | `/api/contratantes/{id}` | Buscar contratante por ID |
| `PUT` | `/api/contratantes/{id}` | Atualizar perfil |
| `GET` | `/api/contratantes/{id}/eventos` | Listar eventos |
| `POST` | `/api/contratantes/evento` | Criar evento |
| `POST` | `/api/contratantes/notificar-artista` | Notificar artista sobre evento |

### Eventos

| Método | Endpoint | Descrição |
|---|---|---|
| `DELETE` | `/api/eventos/{id}` | Cancelar evento |

---

## 🎯 Sistema de Match

O endpoint `/api/artistas/buscar` realiza o match entre eventos e artistas com base em:

- **Formato do show** desejado pelo contratante
- **Distância** via fórmula de Haversine, respeitando o raio do evento
- **Disponibilidade de agenda** na data e horário do evento
- **Equipamento próprio** (opcional)
- **Cache máximo por hora** (opcional)

Os resultados são ordenados do **mais próximo ao mais distante**.

---

## 🐰 Mensageria — RabbitMQ

O `RabbitMqConsumerService` escuta a fila de notificações e envia e-mails via SMTP (MailKit) quando um contratante notifica um artista sobre um evento compatível.

Painel de gerenciamento: [http://localhost:15672](http://localhost:15672) — `guest / guest`

---

## 🧪 Testes

```bash
# Rodar todos os testes
dotnet test

# Apenas unitários
dotnet test tests/MusicMatch.Tests.Unit

# Apenas integração
dotnet test tests/MusicMatch.Tests.Integration
```

### Cobertura atual

| Tipo | Quantidade | Status |
|---|---|---|
| Unitários | 4 | ✅ Passando |
| Integração | 2 | ✅ Passando |
| CI (GitHub Actions) | — | ✅ Automático a cada push |

---

## 🏗️ Padrões e Princípios

| Padrão | Aplicação |
|---|---|
| Clean Architecture | Separação clara de responsabilidades por camada |
| CQRS | Commands e Queries separados via MediatR |
| Repository Pattern | Abstração do acesso a dados |
| DTO Pattern | Objetos de transferência entre camadas |
| SOLID | Aplicado em todas as camadas |

---

## ✅ Status do Projeto

| Funcionalidade | Status |
|---|---|
| Clean Architecture | ✅ |
| CQRS com MediatR | ✅ |
| FluentValidation | ✅ |
| PostgreSQL + EF Core | ✅ |
| RabbitMQ + notificações por e-mail | ✅ |
| Autenticação Google OAuth + JWT | ✅ |
| Sistema de Match (Haversine + filtros) | ✅ |
| Frontend React + TypeScript + Tailwind | ✅ |
| Cadastro e edição de perfil | ✅ |
| Cancelamento de eventos e agendas | ✅ |
| Docker Compose (infraestrutura) | ✅ |
| Testes unitários e de integração | ✅ |
| CI/CD com GitHub Actions | ✅ |

---

## 👨‍💻 Autor

Projeto de estudo e evolução em arquitetura backend com .NET, mensageria, autenticação OAuth e frontend moderno com React.
