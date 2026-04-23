# How to read this project (reverse-engineering guide)

For Gab and Alieya. Use this when you're lost and want to understand how MakauTech works before you change anything.

---

## The big picture

MakauTech is a **.NET MVC web app**. MVC means **M**odel - **V**iew - **C**ontroller. A request flows like this:

```
User clicks link in browser
        ↓
URL routes to a CONTROLLER (e.g. /Home/Index → HomeController.Index())
        ↓
Controller calls a SERVICE (business logic) or queries the DATABASE
        ↓
Controller passes data (the MODEL) to a VIEW
        ↓
VIEW renders HTML and sends it back to browser
```

---

## Folder map

```
MakauTech/
├── Controllers/     ← Entry points. Handle URL requests.
├── Views/           ← HTML templates (.cshtml). What user sees.
├── Models/          ← Data shapes. Plain C# classes.
├── Services/        ← Business logic. Called by controllers.
├── Hubs/            ← Real-time push (SignalR).
├── Data/            ← Database context + migrations.
├── Helpers/         ← Utility functions.
├── wwwroot/         ← Static files (CSS, JS, images).
├── Program.cs       ← App startup. Configuration lives here.
└── appsettings.json ← Connection strings, API keys, etc.
```

---

## How to trace a feature end-to-end (reverse engineering)

**Example: user views a place detail page.**

1. User types URL or clicks a link → `/Place/Detail/5`
2. Open `Controllers/PlaceController.cs`. Find `Detail(int id)`.
3. See what it does. Probably: queries the DB for place with id=5, returns a View.
4. It returns `View(model)`. Where's the view? Default location: `Views/Place/Detail.cshtml`.
5. Open that .cshtml file. See how it uses the model data (`@Model.Name`, etc.).
6. If there's AJAX in the view (fetch calls), trace those URLs back to another controller.

**Do this once. Then you understand how everything connects.**

---

## Keyboard shortcuts in Visual Studio (save time)

| Shortcut | What it does |
|---|---|
| **F12** | Go to definition (jump to where a class/method is defined) |
| **Shift + F12** | Find all usages (who calls this method?) |
| **Ctrl + T** | Quick search file / type / symbol |
| **Ctrl + .** | Show quick fixes (e.g. missing using statement) |
| **Ctrl + K, Ctrl + D** | Auto-format current file |
| **F5** | Run with debugger |
| **Ctrl + F5** | Run without debugger (faster startup) |
| **F9** | Toggle breakpoint on current line |

---

## Three questions to ask before changing any file

1. **What does this file do?** Read it top to bottom.
2. **Who uses this file?** Shift+F12 → "Find all references".
3. **What happens if I break it?** What page or feature stops working?

If you can't answer those 3 questions, **don't change the file yet**. Read more first.

---

## Common patterns in this codebase

### Pattern 1 — Controller action
```csharp
public async Task<IActionResult> Index()
{
    var data = await _service.GetStuffAsync();
    return View(data);
}
```
Meaning: handle the request, get data, render the view.

### Pattern 2 — Database query
```csharp
var places = await _db.Places
    .Where(p => p.IsActive)
    .OrderBy(p => p.Name)
    .ToListAsync();
```
Meaning: get all active places, sorted by name.

### Pattern 3 — Pass data to view
```csharp
// Controller
return View(new PlaceViewModel { Places = places, UserName = user.Name });
```
```cshtml
<!-- View -->
@model PlaceViewModel
<h1>Hello @Model.UserName</h1>
@foreach (var p in Model.Places) { <div>@p.Name</div> }
```

### Pattern 4 — Form submit
```cshtml
<form asp-controller="Feedback" asp-action="Submit" method="post">
    <input name="Message" />
    <button type="submit">Send</button>
</form>
```
```csharp
[HttpPost]
public async Task<IActionResult> Submit(FeedbackModel model) { ... }
```

---

## Debugging tips

- **App crashes on startup?** Look at `Program.cs` and `appsettings.json`. Probably a bad connection string or missing service registration.
- **Page gives 404?** Check the URL matches a controller + action. Default route: `{controller}/{action}/{id?}`.
- **Page gives 500 error?** Set breakpoint in the controller action. Press F5. Step through line by line.
- **"Object reference not set to an instance of an object"?** Something is null. Add `?.` or null checks.
- **Database change not working?** Did you run `dotnet ef database update`?

---

## When you're really stuck

Write a message to Bryan with **exactly** this format:

```
Task: [task ID from _TODO.md]
File: [which file]
What I tried: [what you did]
What I expected: [what should happen]
What happened instead: [the error or wrong behavior]
Error message: [paste it exactly, don't summarize]
```

Vague messages like "it doesn't work" waste everyone's time.

---

## One more thing

**Read the existing code before writing new code.** If you need to add a controller action, find a similar one in the same file and copy its style. Consistency > cleverness.
