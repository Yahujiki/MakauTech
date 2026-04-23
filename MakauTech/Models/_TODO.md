# Models — Alieya's tasks

Data shapes + validation rules.

---

## P0

- [ ] Add validation attributes on **Place.cs**, **Feedback.cs**, **Review.cs**.
  - Required fields: `[Required]`
  - Max length: `[StringLength(200)]`
  - Email: `[EmailAddress]`
  - Range: `[Range(1, 5)]` for star ratings
  - Example:
    ```csharp
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; }
    ```
  - Invalid model → controller returns `400` with `ModelState.Errors`.

## P1

- [ ] **BaseEntity.cs** — make CreatedAt and UpdatedAt auto-set.
  - Override `SaveChangesAsync` in `ApplicationDbContext`:
    ```csharp
    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        foreach (var e in ChangeTracker.Entries<BaseEntity>())
        {
            if (e.State == EntityState.Added) e.Entity.CreatedAt = now;
            if (e.State == EntityState.Modified) e.Entity.UpdatedAt = now;
        }
        return base.SaveChangesAsync(ct);
    }
    ```
  - Now new/edited rows have timestamps without manual code everywhere.

---

## Rules

1. Properties should be `PascalCase`: `UserId`, not `userId`.
2. Use `int?` (nullable) if the field is optional.
3. Navigation properties: `public virtual User User { get; set; }`.
4. Don't put logic in models. Just data + validation attributes.

---

## Quick reference — validation attributes

| Attribute | What it does |
|---|---|
| `[Required]` | Cannot be null or empty |
| `[StringLength(max)]` | Max characters |
| `[StringLength(max, MinimumLength = min)]` | Min + max |
| `[EmailAddress]` | Must be valid email |
| `[Range(min, max)]` | Number in range |
| `[RegularExpression("...")]` | Matches pattern |
| `[Url]` | Must be valid URL |
| `[Compare("OtherField")]` | Must match another field (e.g. confirm password) |
