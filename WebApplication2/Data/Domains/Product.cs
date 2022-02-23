using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Data.Domains;

public class Product
{
    //[Key]
    public  int Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ModificationDate { get; set; }
    [StringLength(100)]
    public string Name { get; set; }
    public string Description { get; set; }
    public  int Price { get; set; }
    [StringLength(50)]
    [Column(TypeName = "varchar")]
    public  string Image { get; set; }

}