﻿using Flunt.Validations;

namespace IWantApp.Domain.Products;

public class Product : Entity
{
    public Product(string name, Category category,
        string description, decimal price, bool hasStock, string createdBy)
    {
        Name = name;
        Category = category;
        Description = description;
        HasStock = hasStock;
        CreatedBy = createdBy;
        EditedBy = createdBy;
        CreatedOn = DateTime.Now;
        EditedOn = DateTime.Now;
        CategoryId = category.Id;
        Price = price;

        Validate();
    }

    public Product()
    {
    }

    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool HasStock { get; private set; }
    public Guid CategoryId { get; private set; }
    public Category Category { get; private set; }
    public bool Active { get; private set; } = true;
    public decimal Price { get; private set; }

    public void Validate()
    {
        var contract = new Contract<Product>()
            .IsNotNullOrEmpty(Name, "Name")
            .IsGreaterOrEqualsThan(Name, 3, "Name")
            .IsNotNull(Category, "Category", "Category not found")
            .IsNotNullOrEmpty(Description, "Description")
            .IsGreaterOrEqualsThan(Description, 3, "Description")
            .IsNotNullOrEmpty(CreatedBy, "CreatedBy")
            .IsNotNullOrEmpty(EditedBy, "EditedBy");

        AddNotifications(contract);
    }

    public void EditInfo(string name, string description, bool hasStock, Category category, string editedBy)
    {
        Name = name;
        Description = description;
        HasStock = hasStock;
        Category = category;
        EditedBy = editedBy;
        EditedOn = DateTime.Now;

        Validate();
    }
}
