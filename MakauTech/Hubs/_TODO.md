# Hubs — Alieya's tasks

SignalR real-time stuff.

---

## P1

- [ ] **NotificationHub.cs** — push real-time events to connected clients.
  - Methods to add:
    - `SendBadgeEarned(userId, badgeName, iconUrl)` — fires when user unlocks a badge.
    - `SendFriendActivity(userIds, message)` — pushes to user's friends.
    - `SendAdminAlert(message)` — pushes to all admins only.
  - Target: client receives the event within 2 seconds of unlock.

---

## How to call from a service

```csharp
public class BadgeService
{
    private readonly IHubContext<NotificationHub> _hub;

    public BadgeService(IHubContext<NotificationHub> hub) { _hub = hub; }

    public async Task UnlockBadgeAsync(int userId, string badgeName)
    {
        // ... save badge to DB ...
        await _hub.Clients.User(userId.ToString())
            .SendAsync("badgeEarned", new { badgeName, iconUrl = "..." });
    }
}
```

## How to receive on client (for Gab's reference, not your task)

```javascript
const conn = new signalR.HubConnectionBuilder()
    .withUrl("/notificationHub").build();

conn.on("badgeEarned", (data) => {
    showToast(`New badge: ${data.badgeName}!`);
});

conn.start();
```

---

## Rules

1. **Authenticate the hub**: add `[Authorize]` on the Hub class.
2. **Don't send sensitive data** — anyone who inspects the network can see it.
3. **Group users** by role or friend-list for targeted push (`Clients.Group("admins")`).
