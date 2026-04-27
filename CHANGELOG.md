# Changelog

All notable changes to MakauTech are recorded here. Newest first. Dates are local (Asia/Kuching).

The format follows [Keep a Changelog](https://keepachangelog.com/en/1.1.0/) and the project uses incremental, additive schema migrations (no destructive `dotnet ef migrations` workflow yet).

---

## [Unreleased]

## [0.5.0] — 2026-04-28

### Added
- **Public Updates feed** (`/Home/Updates`) — admin posts dated announcements visible to all visitors. Latest three appear on the home page.
- **Admin Updates panel** (`/Admin/Updates`) — list, create with cover image, edit, delete. Drafts supported via `IsPublished` flag.
- New `Update` entity in the domain model and ERD.
- Public `README.md` and this `CHANGELOG.md` so the repository tells its own story.

### Changed
- `BaseEntity.Id` and `BaseEntity.CreatedAt` setters opened to `public` so admin code can stamp timestamps explicitly. EF Core hydration unaffected.
- `FactoryReset` now also clears the `Updates` table.

### Fixed
- Leaderboard medal emojis on the home page rendered as solid silhouettes on Windows because `-webkit-background-clip: text` does not work for emoji glyphs. Replaced with a separate rank label and unstyled emoji.
- Games-promo icon strip toned down — removed continuous floating animation in favour of a calm hover lift.

## [0.4.0] — 2026-04-27

### Added
- **Visual polish layer** (`wwwroot/css/polish.css`) — refined Sibu-inspired palette (rainforest emerald, sunset orange, Foochow cream), Fraunces display serif, multi-layer shadows, focus-visible states, custom scrollbar, reduced-motion support.
- **Unified games theme** (`wwwroot/css/games.css`) — `.gm-*` component system: HUD, pay-to-play card, big start button, end-screen result, immersive backdrop. Replaces inline styles duplicated across game views.
- **Admin polish** (`wwwroot/css/admin-polish.css`) — emerald accents on sidebar/topbar, refined stat cards, table hover, button elevation.
- **Per-user rate limit on review submission** (5/min) — prevents review spam.
- Antiforgery token rendered globally in `_Layout` so AJAX endpoints can read it.

### Changed
- Home page (`/Home/Index`) rewritten with asymmetric hero, four-card "Why Sibu" overview, featured spots grid, leaderboard preview, and a logged-in dashboard quick-action strip.
- Login and Register pages redesigned around the polish design system; explicit form labels, autocomplete attributes, `role="alert"` errors.
- Onboarding choice tiles converted from `<div onclick>` to `<button>` (semantic and keyboard accessible).
- `HowItWorks` heading hierarchy normalised (`h1 → h2`).

### Fixed
- `GameController.GetQuestions` could throw `NullReferenceException` if a place had a `null` Description and the random question type asked for a description hint. Defensive fallback added.
- `AdminController.FactoryReset` wrapped in a database transaction with rollback. Confirmation strengthened to require the user to type the literal phrase `RESET`.
- AJAX score submission (`/Game/SubmitScore`) and AI chat (`/api/ai/chat`) now send `X-CSRF-TOKEN` and `RequestVerificationToken` headers.

### Performance
- Place imagery optimised — total image weight reduced from 7.95 MB to 2.70 MB (-66%). Largest savings: `sibu-lake-garden.jpg` 3.6 MB → 183 KB, `wisma-sanyan.jpg` 1.24 MB → 331 KB, `kolo-mee.jpg` 903 KB → 274 KB.

## [0.3.0] — 2026-04-26

### Added
- **Architecture page** (`/Home/Architecture`) — public ERD (Mermaid.js), primary/foreign key reference tables, inheritance hierarchy with TPH explanation, polymorphism examples drawn from the codebase, design pattern walkthrough. Built for academic review.
- `.gitattributes` to mark `.cshtml` as C# for GitHub language statistics and to vendor `wwwroot/lib/**`.
- Updated `Content-Security-Policy` to include the Mermaid CDN.

## [0.2.0] — 2026-04-25

### Added
- Initial public push to GitHub at `https://github.com/Yahujiki/MakauTech`.
- Deployment to Oracle Cloud (Ubuntu VPS, nginx → kestrel, systemd).

### Removed
- Google OAuth flow and dependencies (`Microsoft.AspNetCore.Authentication.Google`).
- Groq AI chat integration. Yenkah retains its rule-based reply set, which already covers the common Sibu topics.

### Security
- Hard-coded credentials and API keys removed from `appsettings.json`. Connection string and admin seed password live in `appsettings.Development.json` for the team; production overrides via environment variables.
- Rate limiting on `/Home/Login` (10 requests / minute / IP) and account lockout after 5 failed attempts.
- Content-Security-Policy and HSTS headers; session cookie hardened to `HttpOnly`, `SameSite=Strict`, `Secure=Always`.
- Antiforgery filter applied globally; XSS in the map popup fixed by escaping all dynamic content.

## [0.1.0] — 2026-04-23

### Added
- Initial project scaffold: ASP.NET Core 8 MVC, EF Core (Pomelo MySQL), BCrypt.
- Core domain — User/Admin (TPH), Place, Category, Review, PlaceLike, Achievement, Feedback.
- Admin panel for places, categories, achievements, reviews, users.
- Five mini-games and the supporting controllers/views.
- Session-based authentication, profile, settings, file upload for avatars and review photos.
