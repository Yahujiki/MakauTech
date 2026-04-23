# wwwroot — Gab's tasks

CSS, JS, images. All frontend assets. Read `README_TEAM.md` at root first.

---

## wwwroot/css/

### P0

- [ ] **theme-senior.css** — check all text has enough contrast (dark text on light bg, or reverse).
  - Use https://webaim.org/resources/contrastchecker/ to check.
  - All text must pass WCAG AA.
  - Make tap targets (buttons, links) at least 44px × 44px so fingers can tap them.

### P1

- [ ] **site.css** — pull out repeated colors and sizes into variables at top.
  - Example:
    ```css
    :root {
      --color-primary: #1F3A5F;
      --color-accent: #FDE7CE;
      --space-sm: 8px;
      --space-md: 16px;
      --font-base: 16px;
    }
    ```
  - Then use `var(--color-primary)` instead of hardcoded `#1F3A5F` everywhere.
  - At least 80% of hardcoded colors replaced.

### P2

- [ ] **admin.css** — use the same variables from `site.css`. Remove duplicate rules.

---

## wwwroot/js/

### P1

- [ ] **site.js** — pull reusable stuff into small functions:
  - `showToast(message)` — shows a popup message that fades out
  - `showModal(content)` — opens a modal dialog
  - `validateForm(formElement)` — checks all fields before submit
- [ ] After pulling these out, remove inline `<script>` tags from `.cshtml` files that do the same thing.

### P2

- [ ] Add a global loading spinner for AJAX calls.
  - Shows a spinner after 300ms if request is still pending.
  - Hides when response comes.

---

## wwwroot/Images/ and wwwroot/uploads/

### P1

- [ ] Compress all images bigger than 200 KB.
  - Use https://squoosh.app/ (free, drag and drop).
  - Save as WebP if possible (smaller). Keep JPG/PNG as fallback for old browsers.
  - Target: Home page total image size under 1.5 MB.

### P0 (accessibility)

- [ ] Every `<img>` tag in every `.cshtml` file must have `alt="..."` text.
  - Decorative images: `alt=""` (empty is fine).
  - Content images: describe what's in the image in 3-8 words.

---

## How to test CSS/JS changes

1. Run the project.
2. Hard refresh browser: `Ctrl+Shift+R` (important — normal refresh keeps old CSS cached).
3. Check the page you changed. Does it look right?
4. Check other pages. Did you break anything?
5. F12 → Console. Any red errors from your JS change?
6. Chrome DevTools → Lighthouse tab → run Accessibility + Performance test. Record score.
7. All good → mark task `[x]`.

---

## Rules

- Don't rename existing CSS classes without checking every `.cshtml` file that uses them.
- Don't delete files without asking.
- Test on Chrome AND on phone view.
- Keep file size small. Don't add giant libraries unless Bryan approves.
