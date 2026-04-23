# Views — Gab's tasks

These are your page files. Edit them directly. Pick a task, put `[x]` when done.

Priority: **P0 first, then P1, then P2.** Read `README_TEAM.md` at root first.

---

## Views/Home/

### P0

- [ ] **Home/Index.cshtml** — polish landing page: hero banner, clear CTA button, responsive grid.
  - Works on phone (375px width), no horizontal scroll.
  - Hero image loads fast (< 2 seconds).
- [ ] **Home/Login.cshtml** — show error messages inline (wrong password, empty field).
  - Error shows in red under the input, not as alert popup.
- [ ] **Home/Register.cshtml** — client-side checks: password min 8 chars, email format, passwords match.
  - Show error before submit if invalid.

### P1

- [ ] **Home/Map.cshtml** — fix marker clustering; add Category filter dropdown.
  - Filter updates map without reloading the page.
- [ ] **Home/Leaderboard.cshtml** — add pagination (20 per page) + filter (week / month / all time).
- [ ] **Home/Profile.cshtml** + **Home/EditProfile.cshtml** — avatar upload with preview before save.
- [ ] **Home/Achievements.cshtml** — show badges in a grid. Locked ones greyed out with lock icon. Hover shows how to unlock.
- [ ] **Home/Explore.cshtml** — place cards in grid, images lazy-load.
  - Use `loading="lazy"` on `<img>`.

### P2

- [ ] **Home/Onboarding.cshtml** — 3-step wizard with progress dots. Skip button works.
- [ ] **Home/About.cshtml / HowItWorks.cshtml / Privacy.cshtml / Terms.cshtml** — make text readable, add clear headings.
- [ ] **Home/Settings.cshtml** — clean up layout, group settings by section.
- [ ] **Home/Games.cshtml** — link to each game with its icon and best score.

---

## Views/Game/, FoodCatch/, KitchenRush/, Sibusprinter/

### P0

- [ ] **Game/Index.cshtml** — hub page with cards for each mini-game. Tap a card → go to that game.

### P1

- [ ] **FoodCatch/Index.cshtml** — add score display on screen, pause button, game-over popup with retry + home.
- [ ] **KitchenRush/Index.cshtml** — visible timer bar, combo indicator, sound on/off toggle (save to localStorage).
- [ ] **Sibusprinter/Index.cshtml** — same polish: score display, pause, game-over screen.

### P2

- [ ] All games — add "Back to Hub" button (top-left). Clicking it mid-game asks "Leave without saving?"

---

## Views/Place/

### P1

- [ ] **Place/Detail.cshtml** — review section: star rating (clickable), character counter on textarea (red when over limit).
- [ ] **Place/EditReview.cshtml** — keep same style as Detail; show original review + edit form.

---

## Views/Admin/

### P1

- [ ] **Admin/_AdminLayout.cshtml** — sidebar collapses on tablet screens; highlight the current page.
- [ ] **Admin/Dashboard.cshtml** — add 4 KPI cards at top: total users, total places, total reviews, games played.
- [ ] **Admin/Places.cshtml** + **CreatePlace.cshtml** + **EditPlace.cshtml** — clean forms, preview image before save, clear Save/Cancel buttons.
- [ ] **Admin/Achievements.cshtml** + **CreateAchievement.cshtml** — form validation, clear labels.
- [ ] **Admin/Categories.cshtml** + **CreateCategory.cshtml** — same as above.
- [ ] **Admin/Reviews.cshtml** — show all reviews in a table, with filter by place and delete button.
- [ ] **Admin/Users.cshtml** — user list with search box, role badge, action buttons.

---

## Views/Shared/

### P0

- [ ] **Shared/_Layout.cshtml** — new global nav bar with logo, menu, login/profile button. Mobile: hamburger menu.
  - Nav stays at top when scrolling (sticky).
  - Hamburger menu opens/closes without error.

### P2

- [ ] **Shared/Error.cshtml** — friendly 404/500 page. Show error code, friendly message, "Back to Home" button.

---

## Views/Feedback/

### P1

- [ ] **Feedback/Index.cshtml** — clean form, character counter, required fields marked with asterisk.
- [ ] **Feedback/Thanks.cshtml** — nice thank-you page with link back home.

---

## General rules for all views

- No inline `style="..."`. Use CSS classes in `wwwroot/css/site.css`.
- Every `<img>` needs `alt="..."` text.
- Every button with only an icon needs `aria-label="..."`.
- Test on phone screen size (375px). Chrome DevTools → device toolbar.
- Zero red errors in browser console.

---

## How to test each page

1. Run the project (F5 in Visual Studio).
2. Open the page in Chrome.
3. Press F12 → Console tab. Must be clean (no red).
4. Press F12 → top-left phone icon. Set to iPhone size.
5. Scroll through. Tap every button. Fill every form.
6. If it all works → mark task `[x]` here.
