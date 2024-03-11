using RedStore.Database.DomainModels;
using System.ComponentModel.DataAnnotations;

namespace RedStore.ViewModels.Product;

public abstract class BaseProductViewModel
{
    [Required(ErrorMessage = "Pls enter name")]
    public string Name { get; set; }

    [Range(1, 1000)]
    public decimal Price { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; }
    public int? CategoryId { get; set; }
    public int[] SelectedColorIds { get; set; }
    public List<Color> Colors { get; set; }
    public int[] SelectedSizeIds { get; set; }
    public List<Size> Sizes { get; set; }
}
