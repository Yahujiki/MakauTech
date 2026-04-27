# MakauTech

Tourism web application for Sibu, Sarawak — discover places, write reviews, play short games, climb a leaderboard.

Built with **ASP.NET Core 8 MVC**, **Entity Framework Core**, and **MySQL** (Pomelo provider). Deployed to an Oracle Cloud VPS behind nginx + kestrel.

---

## Features

- **Places explorer** — curated tourism spots across Sibu (food, heritage, nature, city, adventure) with images, ratings, visit counts, likes, and reviews.
- **Interactive map** — Leaflet + OpenStreetMap, popups with category filtering. All popup content HTML-escaped against XSS.
- **Five mini-games** — Food Catch, Kitchen Rush, Memory, Sibu Sprint, Quiz. Each awards points; daily caps and server-side validation prevent spam.
- **Achievements & leaderboard** — eight badges, point-based ranking. Strategy-pattern badge rules for clean extension.
- **Daily updates feed** — admin posts announcements; visible to public (logged in or not).
- **AI tour guide** — rule-based chatbot ("Yenkah") with 15+ topic branches covering Sibu food, heritage, transport, festivals.
- **Admin panel** — places, categories, reviews, users, achievements, updates; transactional factory reset with typed-confirmation.

## Tech stack

| Layer | Choice |
|---|---|
| Runtime | .NET 8, ASP.NET Core MVC |
| ORM | Entity Framework Core 8 (Pomelo MySQL provider) |
| Database | MySQL 8 (remote, Oracle Cloud) |
| Frontend | Razor views, vanilla JS, Bootstrap 5, custom CSS |
| Auth | Session-based (`HttpContext.Session`) + BCrypt hashing |
| Real-time | SignalR (`/hubs/notifications`) |
| Hosting | Oracle Cloud VPS · nginx · kestrel · systemd |

## Architecture & OOP

The project is intentionally OOP-textbook for academic review:

- **Inheritance** — `Admin` extends `User` (TPH discriminator); every entity inherits `BaseEntity`.
- **Polymorphism** — `is Admin` runtime checks for routing; abstract `GetDisplayInfo()` overridden per entity; abstract `FoodItem` with concrete subclasses; abstract `Challenge` with concrete subclasses.
- **Strategy pattern** — `BadgeService` uses `Func<User,DbContext,bool>` predicates so new badges are pluggable.
- **Filter pattern** — global `AutoValidateAntiforgeryTokenAttribute` for CSRF.

A live walkthrough page is at **/Home/Architecture** with an ERD diagram (Mermaid.js), primary/foreign key tables, and code excerpts.

## Security baseline

- Global anti-forgery filter (every POST validated)
- Per-IP rate limiting on login (10/min); per-user rate limiting on review submission (5/min) and AI chat (30/min)
- Account lockout: 5 failed logins → 15-minute lockout
- BCrypt password hashing (workFactor 12)
- Content-Security-Policy, HSTS, `X-Content-Type-Options`, `X-Frame-Options`, `Referrer-Policy`, `Permissions-Policy`
- Session cookies: `HttpOnly`, `SameSite=Strict`, `Secure=Always`
- File upload whitelist (jpg/png/webp/gif), GUID filenames, size caps (avatars 2 MB, reviews 5 MB, feedback 4 MB, update covers 4 MB)
- All SQL parameterised; no user input in `ExecuteSqlRaw`
- Map popup content HTML-escaped client-side
- Factory reset requires typed `RESET` phrase **and** runs in a transaction with rollback

## Project layout

```
MakauTech/
├── Controllers/       — Admin, Home, Place, Game, FoodCatch, KitchenRush,
│                        Memory, Sibusprinter, Ai, Feedback, Sitemap
├── Models/            — Domain entities + view models (TPH inheritance)
├── Data/              — DbContext, seeder
├── Services/          — BadgeService, GameService, PointsService, TourismService
├── Helpers/           — String/format/validation extensions
├── Hubs/              — SignalR NotificationHub
├── Views/             — Razor views (per controller + Shared/_Layout)
├── wwwroot/           — Static assets (css, js, images, vendored libs)
└── Program.cs         — App startup, DI, middleware, schema patches
```

## Running locally

**Prerequisites**

- Visual Studio 2022 with the *ASP.NET and web development* workload
- .NET 8 SDK
- Internet access (the database is remote)

**Get started**

```sh
git clone https://github.com/Yahujiki/MakauTech.git
cd MakauTech
```

Open `MakauTech.slnx` in Visual Studio → press **F5**.

Default admin login: `admin@makautech.com` / `adminmakautech` *(change after first login)*.

> The dev connection string lives in `appsettings.Development.json` for one-step team onboarding. Production deployments override via environment variables (`ConnectionStrings__DefaultConnection`).

## Team workflow

1. Create a feature branch — `<your-name>/<task-id>-<short-desc>`
2. Pull `main`, write code, commit, push the branch
3. Open a pull request and request review
4. Do not merge your own PR; do not push to `main` directly

Internal task lists, the admin setup guide, and the full upgrade walkthrough are shared separately with the team (PDF/DOCX bundle).

## Changelog

See [CHANGELOG.md](CHANGELOG.md) for a public, dated record of releases and notable changes.

## Licence

Educational project; not licensed for redistribution.
