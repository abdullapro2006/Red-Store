﻿using RedStore.Database.Abstracts;

namespace RedStore.Database.DomainModels;

public class Product : IEntity
{
    public Product()
        :this(default,default,default)
    {

    }
    public Product(string name, decimal price, int rating)
    {
        Name = name;
        Price = price;
        Rating = rating;
        CreatedAt = DateTime.Now;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Rating { get; set; }
    public int? CategoryId { get; set; }

    public Category Category { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }


}
