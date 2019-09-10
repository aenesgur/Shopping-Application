using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Abstract;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public class EfCoreProductDal : EfCoreGenericRepository<Product, ShopContext>, IProductDal
    {
        public void Create(Product entity, int[] categoryIds)
        {
            using (var context = new ShopContext())
            {
                var product = new Product()
                {
                    Name = entity.Name,
                    Description = entity.Description,
                    ImageUrl = entity.ImageUrl,
                    Price = entity.Price,
                    ProductCategories = categoryIds.Select(catid => new ProductCategory()
                    {
                        CategoryId = catid,
                        ProductId = entity.Id
                    }).ToList()
                };
                context.Add(product);
                context.SaveChanges();

            }
        }

        public Product GetByIdWithCategories(int id)
        {
            using (var context = new ShopContext())
            {
                return context.Products
                    .Where(i => i.Id == id)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Category)
                    .FirstOrDefault();
            }
        }

        public int GetCountByCategory(string category)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.AsQueryable();
                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                        .Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Category)
                        .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
                }
                return products.Count();
            }
        }

        public IEnumerable<Product> GetPopulerProducts()
        {
            throw new NotImplementedException();
        }

        public Product GetProductDetails(int id)
        {
            using (var context = new ShopContext())
            {
                return context.Products
                    .Where(i => i.Id == id)
                    .Include(i => i.ProductCategories)
                    .ThenInclude(i => i.Category)
                    .FirstOrDefault();
            }
        }

        public List<Product> GetProductsByCategory(string category, int page, int pageSize)
        {
            using (var context = new ShopContext())
            {
                var products = context.Products.AsQueryable();
                if (!string.IsNullOrEmpty(category))
                {
                    products = products
                        .Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Category)
                        .Where(i => i.ProductCategories.Any(a => a.Category.Name.ToLower() == category.ToLower()));
                }
                return products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public void Update(Product entity, int[] categoryIds)
        {
            using (var context = new ShopContext())
            {
                var product = context.Products
                    .Include(i=>i.ProductCategories)
                    .FirstOrDefault(i => i.Id == entity.Id);

                if (product!=null)
                {
                    product.Name = entity.Name;
                    product.Description = entity.Description;
                    product.ImageUrl = entity.ImageUrl;
                    product.Price = entity.Price;

                    product.ProductCategories = categoryIds.Select(catid => new ProductCategory()
                    {
                        CategoryId= catid,
                        ProductId=entity.Id
                    }).ToList();
                    context.SaveChanges();
                }
            }
        }
        //public void Create(Product entity, int[] categoryIds)
        //{
        //    using (var context = new ShopContext())
        //    {
        //        var product = context.Products
        //            .Include(i => i.ProductCategories)
        //            .FirstOrDefault();

        //        if (product != null)
        //        {
        //            product.Name = entity.Name;
        //            product.Description = entity.Description;
        //            product.ImageUrl = entity.ImageUrl;
        //            product.Price = entity.Price;

        //            product.ProductCategories = categoryIds.Select(catid => new ProductCategory()
        //            {
        //                CategoryId = catid,
        //                ProductId = entity.Id
        //            }).ToList();
        //            context.SaveChanges();
        //        }
        //    }
        //}
    }
}
