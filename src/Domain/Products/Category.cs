using Flunt.Validations;

namespace IWantApp.Domain.Products;

public class Category : Entity
{
    public Category(string name, string createdBy)
    {
        Name = name;
        CreatedBy = createdBy;
        EditedBy = createdBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;
        Active = true;

        Validate();
    }

    public Category()
    {
    }

    public string Name { get; private set; }
    public bool Active { get; private set; }

    public void Validate()
    {
        var contract = new Contract<Category>()
            .IsNotNullOrEmpty(Name, "Name")
            .IsNotNull(EditedBy, "EditedBy")
            .IsNotNull(CreatedBy, "CreatedBy");

        AddNotifications(contract);
    }

    public void EditInfo(string name, bool active, string editedBy, DateTime editedOn)
    {
        Name = name;
        Active = active;
        EditedBy = editedBy;
        EditedOn = editedOn;

        Validate();
    }
}
