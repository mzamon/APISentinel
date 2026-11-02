# API Sentinel

API monitoring and testing dashboard backend for real operational workflows.

## Features

- Health endpoint and OpenAPI document.
- Validated CRUD resource API with clear HTTP status codes.
- In-memory persistence for deterministic demonstrations.
- Automated API tests.
- GitHub Actions build and test workflow.

## Run locally

```powershell
dotnet run --project APISentinel
dotnet test
```

The API is available at the URL printed by `dotnet run`. The OpenAPI document is
available at `/openapi/v1.json`.

## Endpoint checks

`EndpointCheck` resources are managed through the following endpoints:

| Method | Route | Purpose |
| --- | --- | --- |
| `GET` | `/health` | Return service health |
| `GET` | `/api/endpointchecks` | List endpoint checks |
| `GET` | `/api/endpointchecks/{id}` | Retrieve one endpoint check |
| `POST` | `/api/endpointchecks` | Create an endpoint check |
| `PUT` | `/api/endpointchecks/{id}` | Replace an endpoint check |
| `DELETE` | `/api/endpointchecks/{id}` | Remove an endpoint check |

The API is intentionally self-contained so the project can be demonstrated without secrets or external services. Replace the store with EF Core and a configured database during the persistence phase.

## Portfolio evidence

This repository is structured for requirements, API design, business rules, testing, security review, deployment documentation and iterative delivery. Demonstration data must be fictional and credentials must remain outside source control.
