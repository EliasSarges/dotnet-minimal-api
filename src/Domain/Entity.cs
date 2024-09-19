using Flunt.Notifications;

namespace IWantApp.Domain;

public abstract class Entity : Notifiable<Notification>
{
    public Entity()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public string CreatedBy { get; protected set; }
    public DateTime CreatedOn { get; protected set; }
    public string EditedBy { get; protected set; }
    public DateTime EditedOn { get; protected set; }
}
