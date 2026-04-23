namespace MakauTech.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; } = DateTime.Now;
        public abstract string GetDisplayInfo();
        public virtual string GetEntityType() => this.GetType().Name;
    }
}
