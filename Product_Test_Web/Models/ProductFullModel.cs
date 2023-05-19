using Microsoft.Build.Framework;

namespace Product_Test_Web.Models
{
    public class ProductFullModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int? CategoryId { get; set; }

        public string? CategoryName { get; set;}
    }
}
