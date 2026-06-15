📋 MusicMatch

Plataforma para conectar artistas e contratantes de eventos, desenvolvida em ASP.NET Core, utilizando Clean Architecture, MediatR, Entity Framework Core, RabbitMQ para mensageria, autenticação via Google OAuth + JWT e React + TypeScript + Tailwind CSS no frontend.

🚀 Tecnologias Utilizadas

Backend

.NET 8 — Plataforma principal
ASP.NET Core Web API — Camada HTTP
Entity Framework Core 8 — ORM para persistência
PostgreSQL — Banco de dados relacional
MediatR — Implementação do padrão CQRS
FluentValidation — Validação de dados de entrada
RabbitMQ — Mensageria para eventos assíncronos
Google OAuth + JWT — Autenticação e autorização
Swagger (OpenAPI) — Documentação da API

Frontend

React 18 — Biblioteca de interface
TypeScript — Tipagem estática
Tailwind CSS — Estilização utilitária
Vite — Ambiente de build
Axios — Cliente HTTP

Infraestrutura

Docker — Containerização
Docker Compose — Orquestração dos serviços (PostgreSQL e RabbitMQ)

📂 Estrutura do Projeto

musicmatch/
│
├── MusicMatch.API/                  # Camada de Apresentação (Injeção de Dependência e Endpoints)
│   ├── Controllers/                 # Controllers da API
│   │   ├── ArtistasController.cs
│   │   ├── ContratantesController.cs
│   │   └── EventosController.cs
│   ├── Program.cs                   # Configuração de serviços e Middleware
│   ├── appsettings.json             # Configurações de conexão e variáveis (não versionado com segredos reais)
│   └── appsettings.Example.json     # Modelo de configuração
│
├── MusicMatch.Application/          # Regras de Negócio e Casos de Uso
│   ├── Commands/                    # Comandos de escrita (CQRS)
│   ├── Queries/                     # Comandos de leitura (CQRS)
│   ├── Handlers/                    # Orquestração (MediatR)
│   ├── Validators/                  # FluentValidation
│   └── DTOs/                        # Objetos de transferência de dados
│
├── MusicMatch.Domain/                # Núcleo do Sistema (Enterprise Rules)
│   ├── Entities/                    # Entidades (Artista, Contratante, Evento, Agenda, etc.)
│   ├── Enums/                       # Tipos enumerados (FormatoShow, TipoEvento, etc.)
│   └── Interfaces/                  # Contratos de Repositórios
│
├── MusicMatch.Infrastructure/        # Implementações de I/O e Ferramentas Externas
│   ├── Persistence/                 # Entity Framework Core (AppDbContext)
│   ├── Repositories/                # Implementações dos repositórios
│   ├── Messaging/                   # RabbitMQ (produtor e consumidor)
│   └── Migrations/                  # Histórico de banco de dados
│
├── MusicMatch.Authentication/        # JWT e integração com Google OAuth
│
├── MusicMatch.Tests/                 # Testes unitários
│
├── musicmatch-frontend/              # SPA em React (TypeScript)
│
└── docker-compose.yml                # Orquestração (PostgreSQL e RabbitMQ)

🔑 Responsabilidades das Camadas

Camada            | Responsabilidade
------------------|------------------------------------------------------
API               | Receber requisições HTTP e retornar respostas
Application       | Orquestrar regras de negócio (CQRS com MediatR)
Domain            | Entidades e regras de negócio puras
Infrastructure    | Banco de dados, RabbitMQ, repositórios, geocoding
Authentication    | JWT + Google OAuth
Tests             | Testes unitários

⚙️ Pré-requisitos

.NET SDK 8
Node.js 18+
Docker Desktop (com WSL2 integration, se estiver no Windows)
Git

Verifique as instalações:

dotnet --version   # 8.0.x
node --version     # v18+ ou v20.x
docker --version   # 20+

▶️ Como Rodar o Projeto

1️⃣ Clonar o repositório

git clone https://github.com/josefelipesouza/musicmatch.git
cd musicmatch

2️⃣ Configurar o appsettings.json

Copie o arquivo de exemplo e preencha com suas próprias credenciais:

cp MusicMatch.API/appsettings.Example.json MusicMatch.API/appsettings.json

Edite MusicMatch.API/appsettings.json e informe:

- ConnectionStrings.DefaultConnection: usuário/senha do PostgreSQL
- Smtp: credenciais de um e-mail para envio de notificações (recomendado: senha de app do Gmail)
- Authentication.Google.ClientId e ClientSecret: credenciais OAuth criadas no Google Cloud Console
- Authentication.Jwt.Secret: uma chave secreta longa e aleatória, usada para assinar os tokens JWT

⚠️ Nunca commite o appsettings.json com credenciais reais. Ele está no .gitignore.

3️⃣ Subir a infraestrutura (PostgreSQL e RabbitMQ)

docker-compose up -d

Serviço     | URL
------------|---------------------------
PostgreSQL  | localhost:5432
RabbitMQ    | localhost:5672
RabbitMQ UI | http://localhost:15672 (guest/guest)

4️⃣ Iniciar o Backend

Abra um terminal e execute:

dotnet restore
dotnet run --project MusicMatch.API

A API estará disponível em:

http://localhost:5216

Swagger (documentação interativa):

http://localhost:5216/swagger

As migrations do banco são aplicadas automaticamente na inicialização da API.

5️⃣ Iniciar o Frontend

Abra outro terminal e execute:

cd musicmatch-frontend
npm install
npm run dev

O frontend estará disponível em:

http://localhost:5173

🔐 Autenticação

O login é feito exclusivamente via Google OAuth:

- GET /api/auth/google — inicia o fluxo de login com o Google
- GET /api/auth/google/callback — callback do Google, gera o token JWT e redireciona para o frontend

No primeiro acesso, o usuário é direcionado para a tela de cadastro (artista ou contratante) para completar seu perfil.

🗄️ Banco de Dados e Migrations

O projeto utiliza Entity Framework Core com PostgreSQL.

Criar uma nova migration

dotnet ef migrations add NomeDaMigration \
  --project MusicMatch.Infrastructure \
  --startup-project MusicMatch.API

Aplicar migrations manualmente

dotnet ef database update \
  --project MusicMatch.Infrastructure \
  --startup-project MusicMatch.API

As migrations ficam em MusicMatch.Infrastructure/Migrations/

📦 Principais Endpoints da API

Artistas

Método | Endpoint                        | Descrição
-------|----------------------------------|---------------------------------------------
POST   | /api/artistas                    | Cadastrar artista
GET    | /api/artistas/{id}               | Buscar artista por ID
PUT    | /api/artistas/{id}               | Atualizar perfil do artista
GET    | /api/artistas/buscar             | Buscar artistas compatíveis (match) com um evento
POST   | /api/artistas/agenda             | Criar agenda de disponibilidade
GET    | /api/artistas/{id}/agendas       | Listar agendas do artista
DELETE | /api/artistas/agenda/{agendaId}  | Cancelar uma agenda

Contratantes

Método | Endpoint                                  | Descrição
-------|--------------------------------------------|---------------------------------------------
POST   | /api/contratantes                          | Cadastrar contratante
GET    | /api/contratantes/{id}                     | Buscar contratante por ID
PUT    | /api/contratantes/{id}                     | Atualizar perfil do contratante
GET    | /api/contratantes/{id}/eventos             | Listar eventos do contratante
POST   | /api/contratantes/evento                   | Criar evento
POST   | /api/contratantes/notificar-artista        | Notificar artista sobre um evento

Eventos

Método | Endpoint              | Descrição
-------|------------------------|---------------------------------------------
DELETE | /api/eventos/{id}      | Cancelar evento

🎯 Sistema de Match

O endpoint /api/artistas/buscar realiza o casamento entre eventos e artistas com base em:

- Formato do show desejado
- Distância (cálculo via fórmula de Haversine), respeitando o raio de busca do evento
- Disponibilidade de agenda na data e horário do evento
- Cache máximo por hora (opcional)
- Necessidade de equipamento próprio (opcional)

Os resultados são ordenados por distância (do mais próximo ao mais distante).

🐰 Mensageria — RabbitMQ

O RabbitMqConsumer escuta a fila de notificações e envia e-mails via SMTP (MailKit) quando um contratante notifica um artista sobre um evento compatível.

Acesse o painel de gerenciamento:

http://localhost:15672
Usuário: guest
Senha: guest

🧪 Testes

Rodar todos os testes

dotnet test MusicMatch.Tests

🏗️ Padrões e Princípios Utilizados

Clean Architecture — Separação clara de responsabilidades
CQRS — Commands e Queries separados via MediatR
SOLID — Princípios aplicados em todas as camadas
Repository Pattern — Abstração do acesso a dados
DTO Pattern — Objetos de transferência entre camadas

👨‍💻 Autor

Projeto desenvolvido para fins de estudo e evolução em arquitetura backend com .NET, mensageria, autenticação OAuth e frontend moderno com React.

✅ Status

Funcionalidade                          | Status
------------------------------------------|--------
Clean Architecture                        | ✔️
CQRS com MediatR                          | ✔️
FluentValidation                          | ✔️
PostgreSQL + EF Core                      | ✔️
RabbitMQ + notificações por e-mail        | ✔️
Autenticação Google OAuth + JWT           | ✔️
Sistema de Match (Haversine + filtros)    | ✔️
Frontend React + TypeScript + Tailwind    | ✔️
Cadastro e edição de perfil               | ✔️
Cancelamento de eventos e agendas         | ✔️
Docker Compose (infraestrutura)           | ✔️
Testes unitários                          | 🔲 Em expansão
