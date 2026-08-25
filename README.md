<div align="center">

<img src="https://capsule-render.vercel.app/api?type=waving&color=0:1a1a2e,100:16213e&height=200&section=header&text=LugenStore%20API&fontSize=60&fontColor=e0e0e0&fontAlignY=38&desc=A%20Game%20Store%20REST%20API&descAlignY=58&descColor=a0a0b0" />

<br/>

![.NET](https://img.shields.io/badge/.NET_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Postman](https://img.shields.io/badge/Postman-FF6C37?style=for-the-badge&logo=postman&logoColor=white)
![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)

<br/>

> **LugenStore** is a RESTful API for a digital game store — inspired by platforms like Steam.
> Built with **ASP.NET Core 8**, **Entity Framework Core**, and **PostgreSQL**, following a clean layered architecture with DTOs, repositories, and service validation.

<br/>

</div>

---

## 📑 Table of Contents

- [Overview](#-overview)
- [Architecture](#-architecture)
- [Tech Stack](#-tech-stack)
- [Project Structure](#-project-structure)
- [Endpoints](#-endpoints)
- [Getting Started](#-getting-started)
- [Environment Variables](#-environment-variables)
- [Running Migrations](#-running-migrations)
- [Roadmap](#-roadmap)
- [License](#-license)

---

## 🎮 Overview

LugenStore API is the backend of a digital game store platform. It handles the core catalog management — publishers, genres, and games — with full CRUD operations, input validation, and a clean separation between API contracts and database models.

The project was built with scalability in mind: every layer has a single responsibility, all external contracts go through DTOs, and the database is completely abstracted behind repositories.

---

## 🏗 Architecture

The API follows a **layered architecture** pattern:

```
Request → Controller → Service → Repository → Database
                ↑                      ↓
             DTOs                   Models
```

| Layer | Responsibility |
|---|---|
| **Controllers** | Receive HTTP requests, validate input, return responses |
| **Services** | Contain all business rules and validations |
| **Repositories** | Abstract all database access — return Models only |
| **Models** | Represent database entities |
| **DTOs** | Represent what travels through the API (never expose Models directly) |
| **Configurations** | Define EF Core mappings, constraints, and relationships |

---

## 🛠 Tech Stack

| Technology | Purpose |
|---|---|
| ASP.NET Core 8 | Web framework |
| Entity Framework Core 8 | ORM and migrations |
| PostgreSQL 16 | Relational database |
| Docker & Docker Compose | Database containerization |
| Postman | API testing and documentation |
| C# 12 | Language |

---

## 📁 Project Structure

```
LugenStore/
├── LugenStore.Application
│   ├── DTOs
│   │   ├── Auth
│   │   ├── Cart
│   │   ├── Game
│   │   ├── Genre
│   │   ├── Publisher
│   │   └── User
│   ├── Interfaces
│   ├── Services
│   │   └── Auth
│   └── Validators
│
├── LugenStore.Domain
│   ├── Common
│   │   └── Validation
│   ├── Entities
│   ├── Exceptions
│   └── Interfaces
│
├── LugenStore.Infrastructure
│   ├── Persistence
│   │   ├── Configurations
│   │   └── Migrations
│   ├── Repositories
│   └── Security
│       ├── Hash
│       └── Token
│
└── LugenStore.WebAPI
    ├── appsettings.Development.json
    ├── appsettings.json
    ├── docker-compose.yml
    └── Program.cs
```

---

## 📡 Endpoints

### 🎮 Games

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/Games` | List all games |
| `GET` | `/api/Games/{id}` | Get game by ID |
| `POST` | `/api/Games` | Create a new game |
| `PUT` | `/api/Games/{id}` | Update a game |
| `DELETE` | `/api/Games/{id}` | Delete a game |

### 🏷 Genres

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/Genres` | List all genres |
| `GET` | `/api/Genres/{id}` | Get genre by ID |
| `POST` | `/api/Genres` | Create a new genre |
| `PUT` | `/api/Genres/{id}` | Update a genre |
| `DELETE` | `/api/Genres/{id}` | Delete a genre |

### 🏢 Publishers

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/Publishers` | List all publishers |
| `GET` | `/api/Publishers/{id}` | Get publisher by ID |
| `POST` | `/api/Publishers` | Create a new publisher |
| `PUT` | `/api/Publishers/{id}` | Update a publisher |
| `DELETE` | `/api/Publishers/{id}` | Delete a publisher |

### 🔐 Auth

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/Auth/register` | Register a user |
| `POST` | `/api/Auth/login` | User login |

### 👨‍🦱 Users

| Method | Endpoint | Description |
|---|---|---|
| `GET` | `/api/User/{id}` | Get user by ID |
| `PUT` | `/api/User/{id}` | Update user settings |
| `DELETE` | `/api/User/{id}` | Delete a user |

---

### 📦 Example: Create a Game

**POST** `/api/games`

```json
{
  "name": "The Witcher 3",
  "description": "An open-world RPG set in a dark fantasy universe.",
  "price": 59.99,
  "publisherId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "genreIds": [
    "1fa85f64-5717-4562-b3fc-2c963f66afa6",
    "2fa85f64-5717-4562-b3fc-2c963f66afa6"
  ]
}
```

**Response 201 Created**

```json
{
  "id": "9ba85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "The Witcher 3",
  "description": "An open-world RPG set in a dark fantasy universe.",
  "price": 59.99,
  "publisher": "CD Projekt Red",
  "genres": ["RPG", "Open World"],
  "createdAt": "2026-04-06T12:00:00Z"
}
```

> ⚠️ A **Publisher** and at least one **Genre** must be created before registering a Game.

---

### 📦 Example: User Register

**POST** `/api/Auth/Register`

```json
{
  "name": "Jonh Doe",
  "cpf": "12345678911",
  "email": "jonhdoe@email.com",
  "password": "doe123@",
  "confirmPassword": "doe123@"
  ]
}
```

**Response 201 Created**

```json
{
  "id": "2fccb4b6-2e48-40f1-81c1-75ad04ff03c7"
  "name": "Jonh Doe",
  "cpf": "12345678911",
  "email": "jonhdoe@email.com",
  "createdAt": "2026-04-15T16:21:02.0684431Z"
  ]
}
```

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### 1. Clone the repository

```bash
git clone https://github.com/your-username/LugenStore.git
cd LugenStore/LugenStore.API
```

### 2. Start the database

```bash
docker-compose up -d
```

### 3. Apply migrations

```bash
dotnet ef database update
```

### 4. Run the API

```bash
dotnet run
```

### 5. Open Postman

```
https://localhost:7197
```

---

## 🔐 Environment Variables

The connection string is configured in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=lugenstore;Username=lugenstore;Password=your_password"
  }
}
```

The Docker Compose credentials must match:

```yaml
POSTGRES_USER: lugenstore
POSTGRES_PASSWORD: your_password
POSTGRES_DB: lugenstore
```

---

## 🗄 Running Migrations

```bash
# Create a new migration
dotnet ef migrations add MigrationName

# Apply migrations to the database
dotnet ef database update

# Remove the last migration (if not applied)
dotnet ef migrations remove
```

---

## 🗺 Roadmap

- [x] Games CRUD
- [x] Genres CRUD
- [x] Publishers CRUD
- [X] User CRUD
- [x] Layered architecture (Controllers / Services / Repositories)
- [x] DTO pattern (separation between API contracts and database models)
- [x] EF Core Configurations (constraints, relationships, precision)
- [x] Docker Compose for PostgreSQL
- [x] User registration and authentication
- [x] JWT authentication
- [X] Clean Architecture
- [ ] Elastic Search integration
- [ ] Shopping cart with price snapshot
- [ ] Cart expiration logic
- [ ] Observability (structured logging, metrics, tracing)
- [ ] AWS deployment (Lambda + API Gateway)
- [X] CI pipeline
- [ ] CD pipeline

---

## 📄 License

This project is licensed under the **MIT License** — see the [LICENSE](LICENSE) file for details.

---

<div align="center">

<img src="https://capsule-render.vercel.app/api?type=waving&color=0:16213e,100:1a1a2e&height=100&section=footer" />

<sub>Built with dedication by <strong>Luiz Fonseca</strong></sub>

</div>
