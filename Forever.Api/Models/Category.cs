using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Forever.Api.Models
{
    [Table("Category")]
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string CategoryDescription { get; set; } = string.Empty;

        public DateTime CategoryDate { get; set; }

        // IMPORTANT FIX
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}