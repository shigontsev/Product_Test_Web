namespace Product_Test_Web.Models
{
    public class ProductManager
    {
        ApplicationContext db;

        public List<ProductFullModel> GetAllProducts()
        {
            List<ProductFullModel> products = new List<ProductFullModel>();
            using (db = new ApplicationContext())
            {
                if (db.Product.Count() > 0)
                {
                    products = (List<ProductFullModel>)(from product in db.Product
                                                        join category in db.Category on product.CategoryId equals category.Id
                                                        select new ProductFullModel
                                                        {
                                                            Id = product.Id,
                                                            Name = product.Name,
                                                            Description = product.Description,
                                                            CategoryId = product.CategoryId,
                                                            CategoryName = category.Name
                                                        }).ToList();
                }
                return products;
            }
        }

        public List<ProductFullModel> GetProductsByNameOrCategory(string? name, int? categoryId)
        {
            #region mk1
            List<ProductFullModel> products = new List<ProductFullModel>();
            using (db = new ApplicationContext())
            {
                if (db.Product.Count() > 0)
                {
                    if (string.IsNullOrWhiteSpace(name) && (categoryId == 0 || categoryId == null))
                    {
                        products = GetAllProducts();
                        return products;
                    }
                    if (!string.IsNullOrWhiteSpace(name) && categoryId == 0)
                    {
                        products = (List<ProductFullModel>)(from product in db.Product
                                                            join category in db.Category on product.CategoryId equals category.Id
                                                            where product.Name.Contains(name)
                                                            select new ProductFullModel
                                                            {
                                                                Id = product.Id,
                                                                Name = product.Name,
                                                                Description = product.Description,
                                                                CategoryId = product.CategoryId,
                                                                CategoryName = category.Name
                                                            }).ToList();
                        return products;
                    }
                    if (string.IsNullOrWhiteSpace(name) && categoryId != 0)
                    {
                        products = (List<ProductFullModel>)(from product in db.Product
                                                            join category in db.Category on product.CategoryId equals category.Id
                                                            where product.CategoryId == categoryId
                                                            select new ProductFullModel
                                                            {
                                                                Id = product.Id,
                                                                Name = product.Name,
                                                                Description = product.Description,
                                                                CategoryId = product.CategoryId,
                                                                CategoryName = category.Name
                                                            }).ToList();
                        return products;
                    }
                    products = (List<ProductFullModel>)(from product in db.Product
                                                        join category in db.Category on product.CategoryId equals category.Id
                                                        where product.Name.Contains(name) && product.CategoryId == categoryId
                                                        select new ProductFullModel
                                                        {
                                                            Id = product.Id,
                                                            Name = product.Name,
                                                            Description = product.Description,
                                                            CategoryId = product.CategoryId,
                                                            CategoryName = category.Name
                                                        }).ToList();
                }
                return products;
            }
            #endregion mk1


            #region mk2
            //List<ProductFullModel> products = new List<ProductFullModel>();
            //using (db = new ApplicationContext())
            //{
            //    if (db.Product.Count() > 0)
            //    {
            //        products = (List<ProductFullModel>)(from product in db.Product
            //                                            join category in db.Category on product.CategoryId equals category.Id
            //                                            select new ProductFullModel
            //                                            {
            //                                                Id = product.Id,
            //                                                Name = product.Name,
            //                                                Description = product.Description,
            //                                                CategoryId = product.CategoryId,
            //                                                CategoryName = category.Name
            //                                            }).ToList();
            //        if (string.IsNullOrEmpty(name) && (categoryId == 0 || categoryId == null))
            //        {
            //            return products.ToList();
            //        }
            //        if (!string.IsNullOrEmpty(name) && categoryId == 0)
            //        {
            //            return products.Where(x=> x.Name.Contains(name)).ToList();
            //        }
            //        if (string.IsNullOrEmpty(name))
            //        {
            //            return products.Where(x => x.CategoryId == categoryId).ToList();
            //        }
            //        return products.Where(x => x.Name.Contains(name) && x.CategoryId == categoryId).ToList();
            //    }
            //    return products;
            //}
            #endregion 
        }


        public List<Category> GetAllCategory()
        {
            var providers = new List<Category>();
            using (db = new ApplicationContext())
            {
                providers = db.Category.ToList();
            }
            return providers;
        }

        public ProductFullModel GetProductById(int id)
        {
            ProductFullModel productFull = new ProductFullModel();
            using (db = new ApplicationContext())
            {
                productFull = db.Product.Join(db.Category, p => p.CategoryId,
                    c => c.Id,
                    (p, c) => new ProductFullModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        CategoryId = p.CategoryId,
                        CategoryName = c.Name
                    }).FirstOrDefault(x => x.Id == id);

                return productFull;
            }
        }

        public bool AddProduct(ProductFullModel product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                return false;
            }
            product.Name = product.Name.Trim();
            if (!string.IsNullOrWhiteSpace(product.Description))
                product.Description = product.Description.Trim();

            using (db = new ApplicationContext())
            {
                if (db.Product.FirstOrDefault(x => x.Name.Equals(product.Name)) != null)
                {
                    //throw new InvalidOperationException($"Продукт с таким Именем {product.Name}  уже существует");
                    return false;
                }

                db.Product.Add(new Product()
                {
                    Name = product.Name,
                    CategoryId = product.CategoryId,
                    Description = product.Description,
                });
                db.SaveChanges();
            }

            return true;
        }

        public bool DeleteProduct(int id)
        {
            using (db = new ApplicationContext())
            {
                db.Product.Remove(db.Product.First(x => x.Id == id));
                db.SaveChanges();
            }

            return true;
        }

        public bool UpdateProduct(ProductFullModel product)
        {
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                return false;
            }
            product.Name = product.Name.Trim();
            if (!string.IsNullOrWhiteSpace(product.Description))
                product.Description = product.Description.Trim();

            Product product_b = new Product();
            using (db = new ApplicationContext())
            {
                if (db.Product.FirstOrDefault(x => x.Name.Equals(product.Name) && x.Id != product.Id) != null)
                {
                    return false;
                }

                product_b = db.Product.FirstOrDefault(x => x.Id == product.Id);
                product_b.Name = product.Name;
                product_b.Description = product.Description;
                product_b.CategoryId = product.CategoryId;

                db.SaveChanges();
            }

            return true;
        }

        public bool AddCategory(string Name)
        {            
            if (string.IsNullOrWhiteSpace(Name))
            {
                return false;
            }
            Name = Name.Trim();
            using (db = new ApplicationContext())
            {
                if (db.Category.FirstOrDefault(x => x.Name.Equals(Name)) != null)
                {
                    //throw new InvalidOperationException($"Категория с таким Именем {Name}  уже существует");
                    return false;
                }

                db.Category.Add(new Category()
                {
                    Name = Name
                });
                db.SaveChanges();
            }

            return true;
        }

        public string GetNameCategoryById(int id)
        {
            string categoryName = string.Empty;
            using (db = new ApplicationContext())
            {
                categoryName = db.Category.FirstOrDefault(x => x.Id == id).Name;
            }
            return categoryName;
        }

    }
}
