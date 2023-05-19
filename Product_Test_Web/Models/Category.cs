using Microsoft.Build.Framework;

namespace Product_Test_Web.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
