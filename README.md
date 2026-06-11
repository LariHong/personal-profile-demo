# Personal Profile Demo

Interview-demo app for personal profile maintenance.

- Backend: C# ASP.NET Core 10 RESTful API
- Frontend: Vue3 + Vite single file component app under `client/`
- Database: SQL Server through EF Core
- Login: intentionally not included

## What this demonstrates

- RESTful CRUD API design with DTO-based input/output contracts.
- EF Core persistence with a unique national ID constraint.
- Server-side validation for required fields, gender values, phone format, and Taiwan national ID checksum.
- Centralized API exception handling for duplicate data, database failures, and unexpected errors.
- Vue3 form/list workflow with search, create, update, delete, loading state, and user-facing error messages.
- A deliberately small vertical slice that focuses on maintainable interview scope instead of login, roles, or deployment.

## 3-minute demo path

1. Open the Vue app and confirm the profile list loads.
2. Create a profile with a valid Taiwan national ID.
3. Try an invalid Taiwan national ID and confirm the API validation error is shown.
4. Try creating another profile with the same national ID and confirm the duplicate-data error is shown.
5. Search by name, city, district, phone, or national ID.
6. Edit the profile, save it, then delete it.

## Project structure

```text
src/PersonalProfile.Api/
  Controllers/       HTTP endpoints
  Data/              EF Core DbContext and model configuration
  Dtos/              API request/response contracts
  Middleware/        API error response handling
  Models/            Persistence entity
  Services/          Business rules and data operations
client/              Vue3 + Vite source app
tests/               Lightweight validation check project
docs/slices/         Scope and slice planning notes
```

## Run

Run the API:

```powershell
cd src/PersonalProfile.Api
dotnet restore
dotnet run
```

Run the Vue dev server in another terminal:

```powershell
cd client
npm install
npm run dev
```

Open `http://127.0.0.1:5173` while developing. The Vite dev server proxies `/api` to `http://127.0.0.1:5188`.

For a single ASP.NET Core demo app:

```powershell
cd client
npm run build
cd ../src/PersonalProfile.Api
dotnet run
```

Open the URL printed by `dotnet run`. The default launch profile uses `http://localhost:5188`.

The development API URL is configured in both:

- `src/PersonalProfile.Api/Properties/launchSettings.json`
- `client/vite.config.js`

## SQL Server

The default connection string uses LocalDB:

```json
"DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=PersonalProfileDemo;Trusted_Connection=True;TrustServerCertificate=True"
```

Override it with user secrets, environment variables, or `appsettings.Development.json` when using another SQL Server instance.

## API

Base URL during local development: `http://localhost:5188`

- `GET /api/profiles?keyword=...`
- `GET /api/profiles/{id}`
- `POST /api/profiles`
- `PUT /api/profiles/{id}`
- `DELETE /api/profiles/{id}`

Required fields:

- `nationalId`: Taiwan national ID
- `name`
- `gender`: `Male`, `Female`, or `Other`
- `birthday`: `yyyy-MM-dd`
- `city`
- `district`
- `address`
- `phone`

## Validation check

Run the same basic checks that the CI workflow runs:

```powershell
dotnet build PersonalProfileDemo.slnx
dotnet run --project tests/PersonalProfile.ValidationCheck/PersonalProfile.ValidationCheck.csproj
cd client
npm ci
npm run build
```

Run the lightweight validation check:

```powershell
dotnet run --project tests/PersonalProfile.ValidationCheck/PersonalProfile.ValidationCheck.csproj
```

This currently checks representative valid and invalid Taiwan national ID cases. A next interview-grade improvement would be replacing this console check with xUnit or NUnit tests for the validator, service rules, and API endpoints.

## Intentional trade-offs

- Authentication and authorization are excluded because the assignment focuses on personal profile maintenance.
- The app uses SQL Server LocalDB by default for a simple Windows demo setup.
- Startup schema creation uses EF Core for demo convenience. For a production-style handoff, add EF Core migrations and document migration commands.
- Pagination and advanced sorting are excluded until the data set is large enough to justify them.
