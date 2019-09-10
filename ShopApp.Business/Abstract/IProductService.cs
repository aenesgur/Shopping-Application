using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShopApp.Business.Abstract
{
    public interface IProductService
    {
        Product GetById(int id);
        Product GetProductDetails(int id);
        Product GetByIdWithCategories(int id);

        List<Product> GetAll();
        List<Product> GetProductsByCategory(string category,int page, int pageSize);
        List<Product> GetPopularProducts();

        void Create(Product entity);
        void Create(Product entity, int[] categoryIds);
        void Update(Product entity);
        void Update(Product entity, int[] categoryIds);
        void Delete(Product entity);
        int GetCountByCategory(string category);
        
    }
}
