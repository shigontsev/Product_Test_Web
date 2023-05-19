using Product_Test_Web.Models;

namespace Product_Test_Web.ViewModels
{
    public class ProductViewModel
    {
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public List<ProductFullModel> products { get; set; }
    }
}
