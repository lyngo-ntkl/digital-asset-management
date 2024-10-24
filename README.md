# Digital Asset Management
## Introduction
Digital asset management is a project to manage files, folders structure and their permission using C# ASP.NET Core API, PostgreSQL, Redis, RabbitMQ, decorator design pattern, and clean architecture.
## Installation
Add the following part to user secrets or appsettings.json file
```
  "hashing": {
    "saltByteSize": 10,
    "hashByteSize": 10,
    "iteration": 10
  },
  "jwt": {
    "key": "This is the key used to sign and verify json web token, the key size must be greater than 512 bits"
  },
  "schedule": {
    "deletedWaitDays": 30
  }
```
### Manually
Prerequisite:
- ASP.NET Core 8
- PostgreSQL
- RabbitMQ
- Redis
### Docker
Prerequisite: Docker
Run the command:
```
docker compose build
docker compose up
```
