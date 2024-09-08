using Flunt.Validations;

namespace IWantApp.Domain.Products;

public class Category : Entity
{
    public Category(string name, string createdBy, string editedBy)
    {
        Name = name;
        CreatedBy = createdBy;
        EditedBy = editedBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;
        Active = true;

        Validate();
    }

    public string Name { get; private set; }
    public bool Active { get; private set; }


    public void Validate()
    {
        var contract = new Contract<Category>();
        contract.IsNotNullOrEmpty(Name, "Name");
        contract.IsNotNull(EditedBy, "EditedBy");
        contract.IsNotNull(CreatedBy, "CreatedBy");

        AddNotifications(contract);
    }

    public void EditInfo(string name, bool active)
    {
        Name = name;
        Active = active;

        Validate();
    }
}
