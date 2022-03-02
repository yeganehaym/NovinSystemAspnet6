using System.ComponentModel.DataAnnotations;
using DNTPersianUtils.Core;

namespace WebApplication2.Data.Entity;

public class BaseEntity
{
    public BaseEntity()
    {
        CreationDate=DateTime.Now;

        PersianDate = DateTime.Now.ToShortPersianDateString();
    }
    public int Id { get; set; }
    public DateTime CreationDate { get; set; }
    [StringLength(10)]
    public string PersianDate { get; set; }
    public DateTime? ModificationDate { get; set; }
    public bool IsRemoved { get; set; }
}