using Flunt.Validations;

namespace IWantApp.Domain.Products;

public class Category : Entity
{
    public Category(string name, string createdBy, string editedBy)
    {
        var contract = new Contract<Category>();
        contract.IsNotNullOrEmpty(name, "Name");
        contract.IsNotNull(editedBy, "EditedBy");
        contract.IsNotNull(createdBy, "CreatedBy");

        AddNotifications(contract);

        Name = name;
        CreatedBy = createdBy;
        EditedBy = editedBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;
        Active = true;
    }

    public string Name { get; set; }
    public bool Active { get; set; }
}
