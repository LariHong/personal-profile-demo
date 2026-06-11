# Personal Profile CRUD Slice Plan

## Planning Mode

greenfield_plan

## Goal

Build an interview-demo web app with ASP.NET Core 10 RESTful API, Vue3 UI, and SQL Server persistence for personal profile maintenance. No login page is included.

## Assumptions

- This is a new product demo under the current workspace.
- The Vue3 source app lives under `client/` and can be built into the ASP.NET Core `wwwroot` for a single-app demo.
- SQL Server defaults to LocalDB and can be changed through `ConnectionStrings:DefaultConnection`.
- Taiwan national ID validation is required on create and update.
- Authentication, authorization, deployment, and advanced audit logging are out of scope.

## Target Repo Scope

personal-profile-demo

## Spec Coverage Inventory

- user_visible_flows: list/search, create, edit, delete personal profiles; no login page.
- data_lifecycle: validate input, persist to SQL Server, query ordered profiles, update records, delete records.
- external_integrations: SQL Server only.
- generated_content_inputs: none.
- api_contracts: REST endpoints under `/api/profiles` with JSON DTOs and validation errors.
- persistence_and_migrations: EF Core SQL Server context and startup schema creation.
- security_and_secrets: no authentication by request; connection string remains config-based and no secrets are committed.
- observability_and_failure: default ASP.NET Core logging plus explicit API error responses for validation/conflict/not found.
- non_functional_requirements: simple interview demo, readable UI, standard Vue3 + Vite source structure.
- deferred_or_explicitly_excluded: login, role permissions, pagination, server-side full-text search, production deployment.

## Coverage Gaps And Handling

- SQL Server availability is environment-dependent.
  - handling: acceptance
  - recommended_action: document LocalDB default and connection string override.
  - location: S1 acceptance and README.

## First Slice

S1. Full personal profile CRUD demo

## Slices

### S1. Full personal profile CRUD demo

- user_flow: User opens the app, sees profile list, searches by keyword, creates a profile, edits it, deletes it, and sees validation errors for invalid Taiwan national ID.
- files_or_areas: ASP.NET Core API, EF Core SQL Server persistence, Vue3 + Vite client, README.
- acceptance:
  - API exposes query, create, update, delete endpoints for personal profiles.
  - Create and update validate Taiwan national ID, name, gender, birthday, city, district, address, and phone.
  - Duplicate national ID is rejected.
  - Vue3 UI can perform query, create, edit, and delete without a login page.
  - SQL Server connection string is configurable.
- not_included: login, roles, pagination, production auth, deployment scripts.
- reason_first: This is the smallest interview-ready vertical slice that demonstrates the required technology stack and all requested maintenance actions.

## Deferred

- Add formal automated tests for validator and API endpoints.
- Add pagination and server-side sorting if the data set grows.
- Add deployment profile and production-grade secret management.

## Handoff To Build And Learn Loop

```yaml
next_skill: build-and-learn-loop
artifact_path: docs/slices/personal-profile-crud-slices.md
first_slice: S1. Full personal profile CRUD demo
```
