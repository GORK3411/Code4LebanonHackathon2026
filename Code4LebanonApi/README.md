Code4LebanonApi — Dashboard metrics + background updater

This repository contains the Code4LebanonApi web API plus a background updater and unit tests to support a learner-dashboard.

Overview of key files added or modified

- Controllers/HelperController.cs: Added endpoints under `GET /api/helper/*` for the dashboard metrics:
  - `registrations-by-channel` — breakdown by Universities, Syndicates, Public Sector, NGOs, Employers, Other.
  - `deep-dive?contains={text}&top={n}` — deep-dive by `utm_source` text.
  - `registrations-growth?from={yyyy-MM-dd}&to={yyyy-MM-dd}` — daily time-series counts.
  - `areas-of-interest` — aggregated areas of interest parsed from responses.
  - `motivations` — aggregated motivations counts.
  - `challenges` — aggregated challenges counts.
  - `channel-effectiveness` — list of objects {channel, region, count}.
  - `region-distribution` — counts per region used for gap analysis.
  - Existing helper endpoints were preserved: `applicants-per-region`, `published-surveys`, `active-published-surveys`, `responses/{surveyId}`.

- Models/ResponseClass.cs: Extended `SurveyResponse` to include parsed fields `AreasOfInterest`, `Motivation`, and `Challenges` for efficient querying.

- Services/Code4LebanonRepository.cs: Added repository methods to compute the metrics needed by the dashboard, including:
  - `GetRegistrationsByChannelAsync()`
  - `GetDeepDiveByEntityAsync(string contains, int top)`
  - `GetRegistrationsGrowthAsync(DateTime from, DateTime to)`
  - `GetAreasOfInterestAsync()`
  - `GetMotivationsAsync()`
  - `GetChallengesAsync()`
  - `GetChannelEffectivenessByRegionAsync()`
  - `GetRegionDistributionAsync()`

- Services/NumuSurveyService.cs: Small fix to method signature for `GetDataAsync()` to return a `List<Survey>`.

- Services/DataRefreshService.cs: Background hosted service that runs every 15 minutes to:
  - Pull pages of responses from the Numu API between the last-known response date and now.
  - Upsert new responses and update existing ones, extracting `areas_of_interest`, `motivation`, and `challenges` when available.
  - Purge old responses older than 2 years to prevent unbounded DB growth.

- Program.cs: Registered the `DataRefreshService` as a hosted service and preserved existing service registrations.

- Tests/Code4LebanonApi.Tests: New xUnit test project (net9.0) with tests for repository methods using EF Core InMemory provider:
  - `RepositoryTests.cs` contains tests for registrations by channel, areas of interest parsing, region distribution, and growth time-series.

How to run

1) Build:

```powershell
dotnet build
```

2) Run the API locally (example URL):

```powershell
$env:ASPNETCORE_URLS="http://localhost:5003"; dotnet run --urls "http://localhost:5003"
```

3) Run tests:

```powershell
dotnet test
```

Notes & next steps

- If using a SQL Server DB (not InMemory), create and apply EF migrations to add the new columns in `SurveyResponses` (AreasOfInterest, Motivation, Challenges).
- The `DataRefreshService` extracts fields defensively from `responses` dictionary — adjust keys as necessary to match the source payload.
- The purge cutoff (2 years) and refresh interval (15 minutes) are configurable by editing `DataRefreshService`.

If you want, I can:
- Add EF Core migrations for a SQL Server schema.
- Extend tests to cover `DataRefreshService` behavior (requires mocking `NumuSurveyService`).
- Provide sample JSON responses for each endpoint.
