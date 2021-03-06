﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;

        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public IActionResult ProductList()
        {
            return View(new ProductListModel()
            {
                Products = _productService.GetAll()
            });
        }

        public IActionResult CreateProduct()
        {
            ViewBag.Categories = _categoryService.GetAll();
            return View(new ProductModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductModel model, int[] categoryIds, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    Price = model.Price,
                    Description = model.Description,                   
                };

                if (file != null && file.Length > 0)
                {
                    var extention = Path.GetExtension(file.FileName);
                    if (extention == ".jpg" || extention == ".png" || extention == ".jpeg")
                    {
                        var randomNames = Path.GetRandomFileName();
                        var filename = Path.ChangeExtension(randomNames, ".jpg");
                        entity.ImageUrl = filename;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", filename);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    else
                    {

                        return RedirectToAction("FileUploadErrorPage", "Error");
                    }
                }
                else
                {
                    return RedirectToAction("FileUploadErrorPage", "Error");

                }

                _productService.Create(entity, categoryIds);
                return RedirectToAction("ProductList");
            }
            return View(model);
                     
        }

        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return null;
            }
            var entity = _productService.GetByIdWithCategories((int)id);
            if (entity == null)
            {
                return NotFound();
            }
            var model = new ProductModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                ImageUrl = entity.ImageUrl,
                Price = entity.Price,
                SelectedCategories=entity.ProductCategories.Select(i=>i.Category).ToList()
            };
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel model, int[] categoryIds,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var entity = _productService.GetById(model.Id);
                if (entity == null)
                {
                    return NotFound();
                }
                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Price = model.Price;

                if (file!=null &&file.Length>0)
                {
                    var extention = Path.GetExtension(file.FileName);
                    if (extention==".jpg" || extention==".png" || extention == ".jpeg")
                    {
                        var randomNames = Path.GetRandomFileName();
                        var filename = Path.ChangeExtension(randomNames,".jpg");
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", filename);
                        entity.ImageUrl = filename;
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                    }
                    else
                    {
                        return RedirectToAction("FileUploadErrorPage", "Error");
                    }

                }
                else
                {
                    return RedirectToAction("FileUploadErrorPage", "Error");
                }

                _productService.Update(entity, categoryIds);
                return RedirectToAction("ProductList");
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
            
        }

        [HttpPost]
        public IActionResult DeleteProduct(int productId)
        {
            var entity = _productService.GetById(productId);
            if (entity != null)
            {
                _productService.Delete(entity);
            }
            return RedirectToAction("ProductList");
        }

        public IActionResult CategoryList()
        {
            return View(new CategoryListModel()
            {
                Categories = _categoryService.GetAll()
            });
        }

        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            var entity = new Category()
            {
                Name = model.Name
            };
            _categoryService.Create(entity);
            return RedirectToAction("CategoryList");
        }

        public IActionResult EditCategory(int id)
        {
            var entity = _categoryService.GetByIdWithProducts(id);

            return View(new CategoryModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Products = entity.ProductCategories.Select(p => p.Product).ToList()
            });
        }

        [HttpPost]
        public IActionResult EditCategory(CategoryModel model)
        {
            var entity = _categoryService.GetById(model.Id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Name = model.Name;
            _categoryService.Update(entity);
            return RedirectToAction("CategoryList");
        }


        [HttpPost]
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);
            if (entity != null)
            {
                _categoryService.Delete(entity);
            }
            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteFromCategory(int categoryId,int productId)
        {
            _categoryService.DeleteFromCategory(categoryId, productId);
            return Redirect("/admin/editcategory/"+categoryId);
        }

    }
}