# API Sentinel

API monitoring and testing dashboard backend for real operational workflows.

## Included baseline

- Health endpoint and OpenAPI document.
- Validated CRUD resource API with clear HTTP status codes.
- In-memory persistence for deterministic demonstrations.
- Automated API tests.
- GitHub Actions build and test workflow.

## Run locally

``powershell
dotnet run --project API Sentinel
dotnet test
``

The API is intentionally self-contained so the project can be demonstrated without secrets or external services. Replace the store with EF Core and a configured database during the persistence phase.

## Portfolio evidence

This repository is structured for requirements, API design, business rules, testing, security review, deployment documentation and iterative delivery. Demonstration data must be fictional and credentials must remain outside source control.
