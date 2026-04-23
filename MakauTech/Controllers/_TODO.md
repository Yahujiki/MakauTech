# Controllers — Alieya's tasks

Edit these files. Pick a task, put `[x]` when done.

Priority: **P0 first, then P1, then P2.** Read `README_TEAM.md` at root first.

---

## P0 (Security — do these FIRST)

- [ ] **AdminController.cs** — add `[Authorize(Roles = "Admin")]` to every action method.
  - Open file. At top of each `public IActionResult ...` method, add the attribute.
  - Test: log in as normal user, try to visit `/Admin/Dashboard`. Must get 403 or redirect to login.

- [ ] **AiController.cs** — add rate limiting + prompt size cap.
  - Reject if user sends more than 30 requests per minute → return `429 TooManyRequests`.
  - Reject if prompt longer than 4000 characters → return `400 BadRequest`.
  - Use `IMemoryCache` to track request counts per user.

- [ ] **FeedbackController.cs** — sanitize user input to prevent XSS.
  - Use `HtmlEncoder.Default.Encode(input)` before saving to DB.
  - Also save `CreatedAt` (timestamp) and `UserId` on the row.

- [ ] **FoodCatchController.cs** — server-side score validation.
  - Calculate max possible score for the game duration.
  - If submitted score > max possible → reject with `400 BadRequest`.
  - Example: if game is 60 seconds and max 10 points per second, reject scores > 600.

---

## P1 (Quality + Structure)

- [ ] **HomeController.cs** — move business logic out to a `HomeService`.
  - Controller actions should be short (under 20 lines).
  - Actions: check login → call service → return view. That's it.

- [ ] **GameController.cs** — unify score submission.
  - Make one endpoint: `POST /api/game/score` with body `{ gameId, score, userId }`.
  - Handles all 4 games (FoodCatch, KitchenRush, Memory, Sibusprinter).
  - Calls `PointsService.Award()` internally.

- [ ] **KitchenRushController.cs** — fix race condition on concurrent score submits.
  - Use `SemaphoreSlim` or DB transaction to prevent lost/duplicate scores.
  - Test: hit endpoint 50 times at once with a load tool. All should save correctly.

- [ ] **MemoryController.cs** — store best time per user.
  - Add endpoint `GET /memory/leaderboard` → returns JSON array of top 10 (username + best time).

- [ ] **PlaceController.cs** — add search endpoint with pagination.
  - `GET /place/search?q=text&page=1&pageSize=20`
  - Returns JSON: `{ results: [...], totalCount: N, page: 1 }`.

- [ ] **PlaceController.cs** — make like toggle idempotent.
  - `POST /place/{id}/like` — if already liked, unlike. If not, like.
  - Return current like count in response.

## P2

- [ ] **SibusprinterController.cs** — validate inputs, return proper HTTP codes.
  - Bad input → `400` with error message. Success → `200` with data.

- [ ] **SitemapController.cs** — generate `/sitemap.xml` from DB.
  - Include all public place detail URLs.
  - Valid XML (test with https://www.xmlvalidation.com/).

---

## Rules for every controller

1. **Thin controllers.** Business logic goes in `Services/`, not here.
2. **Every POST action** needs `[ValidateAntiForgeryToken]` attribute (unless it's a documented API).
3. **Every admin action** needs `[Authorize(Roles = "Admin")]`.
4. **Use async/await** on all database calls. No `.Result`, no `.Wait()`.
   - Right: `var user = await _db.Users.FindAsync(id);`
   - Wrong: `var user = _db.Users.Find(id);`
5. **Never return raw exceptions** to the user.
   - Use try/catch, log the error, return friendly message.

---

## How to test each controller

### Option 1 — Browser (simple)
Just visit the URL in browser. Works for GET endpoints.

### Option 2 — Postman / Swagger (proper)
1. Open Postman (download free).
2. New request. Set method (GET/POST/PUT/DELETE).
3. URL: `http://localhost:PORT/controllerName/actionName`.
4. For POST: Body tab → raw → JSON → paste test data.
5. Send. Check status code and response.

### What to check
- Happy path: normal input → expected result, status `200`.
- Bad input: missing field, wrong type → status `400` with error message.
- No auth: not logged in → status `401` or redirect.
- Wrong role: logged in but not admin → status `403`.

When all 4 pass → mark task `[x]`.

---

## Reverse-engineering tip

Don't know what a controller does? Here's how to figure it out:

1. Look at the file name. `FoodCatchController` → handles the FoodCatch game.
2. Look at the actions (`public IActionResult ...`). Each one is a URL.
3. Look at the View it returns. `return View()` → look in `Views/FoodCatch/`.
4. Look at the Model it uses. Jump to definition (F12 in Visual Studio).
5. Now you know: URL → Controller → Service (if any) → Model → View.

Follow this pattern for every controller you touch.
