# Fintech MiniWallet

A .NET 10 fintech wallet API built with Clean Architecture and CQRS.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://docs.docker.com/get-docker/) and Docker Compose
- [pre-commit](https://pre-commit.com/) (for Git hooks)

## Getting Started

### 1. Clone and install Git hooks

```bash
git clone <repo-url>
cd fintech-miniwallet

# Install pre-commit (Ubuntu/Debian)
sudo apt install pre-commit

# Activate the hooks
pre-commit install
pre-commit install --hook-type pre-push
```

### 2. Restore dependencies

```bash
dotnet restore
```

### 3. Start infrastructure

```bash
docker compose up -d database redis rabbitmq
```

This starts:
- **PostgreSQL 17** on port `5432`
- **Redis 8** on port `6379`
- **RabbitMQ 4** on ports `5672` (AMQP) and `15672` (management UI)

### 4. Run the API locally

```bash
dotnet run --project src/Fintech.MiniWallet.Api
```

The API will be available at `http://localhost:8080`.

### 5. Run with Docker (full stack)

```bash
docker compose up -d
```

This builds and starts the API alongside all infrastructure services.

## Development

### Run tests

```bash
# Unit tests
dotnet test tests/Fintech.MiniWallet.UnitTests

# Integration tests (requires infrastructure running)
dotnet test tests/Fintech.MiniWallet.IntegrationTests

# All tests
dotnet test
```

### Lint / format

```bash
# Check formatting
dotnet format --verify-no-changes --verbosity minimal

# Apply formatting
dotnet format
```

## Git Hooks

The project uses [pre-commit](https://pre-commit.com/) to enforce quality gates automatically. Hooks must be installed once per clone (see [Getting Started](#getting-started)).

| Stage | Hook | What it does |
|-------|------|--------------|
| `pre-commit` | gitleaks | Scans for hardcoded secrets |
| `pre-commit` | dotnet-format | Blocks commit if code is not formatted |
| `pre-push` | dotnet-build | Ensures the solution builds in Release |
| `pre-push` | dotnet-test | Runs the full test suite |

### Verify hooks manually

```bash
# Run all pre-commit hooks against every file
pre-commit run --all-files

# Run only gitleaks
pre-commit run gitleaks --all-files
```

> **Note:** `dotnet-format` verifies but does not auto-fix. If a commit is blocked, run `dotnet format` and stage the changes before committing again.

## Stopping and Cleaning Up

### Stop services (keep data)

```bash
docker compose stop
```

### Stop and remove containers

```bash
docker compose down
```

### Remove containers, volumes, and images

```bash
docker compose down --volumes --rmi local
```

## Project Structure

```
src/
  Fintech.MiniWallet.Domain/          # Entities, value objects, domain events
  Fintech.MiniWallet.Application/     # CQRS commands, queries, handlers
  Fintech.MiniWallet.Infrastructure/  # Database, cache, messaging
  Fintech.MiniWallet.Api/             # HTTP layer, controllers, DI wiring
tests/
  Fintech.MiniWallet.UnitTests/
  Fintech.MiniWallet.IntegrationTests/
```
