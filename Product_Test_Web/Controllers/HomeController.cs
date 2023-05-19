using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
using Product_Test_Web.Models;
using Product_Test_Web.ViewModels;
using System.Diagnostics;

namespace Product_Test_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private ProductManager _pM = new ProductManager();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //public IActionResult Index(string? name, int? categoryId)
        //{
        //    var categorys = new List<Category>
        //    {
        //        new Category()
        //        {
        //            Id = 0,
        //            Name = "All"
        //        }
        //    };
        //    categorys.AddRange(_pM.GetAllCategory());

        //    //ViewBag.Categorys = new SelectList(_pM.GetAllCategory(), "Id", "Name");
        //    ViewBag.Categorys = new SelectList(categorys, "Id", "Name");

        //    //List<ProductFullModel> products;

        //    //if (string.IsNullOrEmpty(name) && (categoryId == 0 || categoryId == null))
        //    //{
        //    //    products = _pM.GetAllProducts();
        //    //    return View(products);
        //    //}
        //    //if (!string.IsNullOrEmpty(name) && categoryId == 0)
        //    //{
        //    //    products= _pM.GetAllProducts().Where(x=>x.Name.Contains(name)).ToList();
        //    //    return View(products);
        //    //}
        //    //if (string.IsNullOrEmpty(name))
        //    //{
        //    //    products = _pM.GetAllProducts().Where(x => x.CategoryId == categoryId).ToList();
        //    //    return View(products);
        //    //}
        //    //products = _pM.GetAllProducts().Where(x => x.Name.Contains(name) && x.CategoryId == categoryId).ToList();

        //    List<ProductFullModel> products = _pM.GetProductsByNameOrCategory(name, categoryId);

        //    return View(products);
        //}

        public IActionResult Index(string? name, int? categoryId)
        {
            var categorys = new List<Category>
            {
                new Category()
                {
                    Id = 0,
                    Name = "All"
                }
            };
            categorys.AddRange(_pM.GetAllCategory());

            //ViewBag.Categorys = new SelectList(_pM.GetAllCategory(), "Id", "Name");
            ViewBag.Categorys = new SelectList(categorys, "Id", "Name");

            //List<ProductFullModel> products;

            //if (string.IsNullOrEmpty(name) && (categoryId == 0 || categoryId == null))
            //{
            //    products = _pM.GetAllProducts();
            //    return View(products);
            //}
            //if (!string.IsNullOrEmpty(name) && categoryId == 0)
            //{
            //    products= _pM.GetAllProducts().Where(x=>x.Name.Contains(name)).ToList();
            //    return View(products);
            //}
            //if (string.IsNullOrEmpty(name))
            //{
            //    products = _pM.GetAllProducts().Where(x => x.CategoryId == categoryId).ToList();
            //    return View(products);
            //}
            //products = _pM.GetAllProducts().Where(x => x.Name.Contains(name) && x.CategoryId == categoryId).ToList();

            List<ProductFullModel> products = _pM.GetProductsByNameOrCategory(name, categoryId);

            var model = new ProductViewModel();
            model.products = products;
            model.Name = string.IsNullOrWhiteSpace(name)? "" : name;
            model.CategoryId = (int)((categoryId == 0 || categoryId == null) ? 0 : categoryId);

            return View(model);
        }

        public IActionResult InfoCategorys()
        {
            var categories = _pM.GetAllCategory();
            return View(categories);
        }


        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(Category model)
        {
            var a = _pM.AddCategory(model.Name);
            //return View(products);
            //return RedirectToAction("~/Home/About", new { messege = "" });
            if (a)
                return RedirectToAction("Message", "Home", new { 
                    message = $"Добавление категории успешно. Категория с Именем \"{model.Name}\"  создана",
                    flag = true
                });
            return RedirectToAction("Message", "Home", new { 
                message = $"Добавление категории не удалось. Категория с таким Именем \"{model.Name}\"  уже существует или обладает пустое значение",
                flag = false
            });
        }

        public IActionResult AddProduct()
        {
            ProductFullModel model = new ProductFullModel();

            ViewBag.Categorys = new SelectList(_pM.GetAllCategory(), "Id", "Name");

            return View(model);
        }

        [HttpPost]
        public IActionResult AddProduct(ProductFullModel model)
        {
            var a = _pM.AddProduct(model);
            if (a)
                return RedirectToAction("Message", "Home", new { 
                    message = $"Добавление продукта успешно. Продукт с Именем \"{model.Name}\"  создана",
                    flag = true
                });
            return RedirectToAction("Message", "Home", new { 
                message = $"Добавление продукта не удалось. Продукт с таким Именем \"{model.Name}\"  уже существует или обладает пустое значение",
                flag = false
            });
        }
                
        public IActionResult DeleteProduct(int id)
        {
            var a = _pM.DeleteProduct(id);

            return RedirectToAction("Index");
        }

        public IActionResult EditProduct(int id)
        {
            ProductFullModel productFull = _pM.GetProductById(id);

            ViewBag.Categorys = new SelectList(_pM.GetAllCategory(), "Id", "Name");

            return View(productFull);
        }

        [HttpPost]
        public IActionResult EditProduct(ProductFullModel model)
        {
            var a = _pM.UpdateProduct(model);
            if (a)
                return RedirectToAction("Message", "Home", new { 
                    message = $"Изменение продукта успешно. Продукт с Именем \"{model.Name}\"  обнавлен",
                    flag = true
                });
            return RedirectToAction("Message", "Home", new { 
                message = $"Изменение продукта не удалось. Продукт с таким Именем \"{model.Name}\"  уже существует или обладает пустое значение",
                flag = false
            });
        }

        public IActionResult Message(string message, bool flag)
        {
            //ViewBag.Message = message;
            var m = new MessageViewModel { 
                Message = message,
                Flag = flag
            };
            return View(m);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}