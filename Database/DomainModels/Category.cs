using RedStore.Database.Abstracts;

namespace RedStore.Database.DomainModels;

public class Category : IEntity
{

    public Category()
    {
        
    }
    public Category(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; set; }
    public string Name { get; set; }
}
