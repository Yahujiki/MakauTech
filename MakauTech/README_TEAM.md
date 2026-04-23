# MakauTech — Team Guide

Hi Gab, hi Alieya. Read this first. Takes 5 minutes.

---

## Who does what

| You | You own | You don't touch |
|---|---|---|
| **Gab** (Frontend) | `Views/` · `wwwroot/` (css, js, images) | `Controllers/` · `Services/` · `Models/` · `Program.cs` |
| **Alieya** (Controller) | `Controllers/` · `Services/` · `Models/` · `Hubs/` · `Data/` · `Program.cs` · `appsettings.json` | `Views/` · `wwwroot/` |

If you need something from the other side, ping each other. Don't edit the other person's folder.

---

## How to start (both of you)

1. Pull the latest code.
2. Open the folder you own.
3. Find the file called `_TODO.md` inside that folder.
4. Pick a task marked **P0** first.
5. Make a branch: `feature/<your-initial>-<short-name>` (example: `feature/G-home-polish`).
6. Fix / build / test.
7. Commit with a clear message. Push. Open a PR. Tag Bryan.
8. Move to next task.

---

## Priority meaning

- **P0** = must be done before we ship. Do these first.
- **P1** = important. Do after P0.
- **P2** = nice to have. Only if time left.

Rule: finish all P0 before you start any P1. No cherry-picking easy P2s.

---

## What "done" means

Before you mark a task done:

- Code builds with no warnings.
- No red errors in browser console (Frontend) or in logs (Backend).
- You tested it yourself. On phone size too (Frontend).
- Commit message has the task name.
- You reviewed your own diff before pushing.

---

## Where to find the tasks

Each folder has a `_TODO.md` file:

- **Gab:**
  - `Views/_TODO.md` — all your page tasks
  - `wwwroot/_TODO.md` — CSS, JS, image tasks
- **Alieya:**
  - `Controllers/_TODO.md` — controller tasks
  - `Services/_TODO.md` — service tasks
  - `Models/_TODO.md` — model + validation tasks
  - `Data/_TODO.md` — database tasks
  - `Hubs/_TODO.md` — SignalR tasks

Open the folder → open `_TODO.md` → pick a task → do it. Simple.

---

## Rules for both of you

1. **Don't break what works.** If you change a file, test that page/endpoint still works.
2. **Small commits.** One task = one branch = one PR.
3. **Ask early.** If you're stuck for 30 minutes, ask. Don't burn a day.
4. **No secrets in code.** No passwords, no API keys in files you commit.
5. **Mobile first** (Gab). Most users are on phone.
6. **Server always validates** (Alieya). Never trust what the browser sends.

---

## If you're stuck

- Read the file you're working in. Understand what it does before changing it.
- Look at similar files for the same pattern. Example: if you're editing `PlaceController`, look at `HomeController` for style.
- Google the error message. Stack Overflow is your friend.
- Still stuck? Ping Bryan with: what you tried, what error you got, what file.

---

## Deadline target

2 weeks. Week 1 = all P0 done. Week 2 = P1 + bug fixing.
