using RedStore.Database.Abstracts;
using System.ComponentModel.DataAnnotations.Schema;

namespace RedStore.Database.DomainModels;

[Table("categories")]
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

    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    public string Name { get; set; }
}
