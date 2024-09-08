using Flunt.Validations;

namespace IWantApp.Domain.Products;

public class Category : Entity
{
    public Category(string name)
    {
        var contract = new Contract<Category>();
        contract.IsNotNull(name, "Name");

        AddNotifications(contract);

        Name = name;
        Active = true;
    }

    public string Name { get; set; }
    public bool Active { get; set; }
}
