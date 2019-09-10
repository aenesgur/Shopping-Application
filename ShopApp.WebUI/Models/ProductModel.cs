using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.WebUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "İsim alanı boş geçilemez")]
        [StringLength(60, MinimumLength = 10, ErrorMessage = "Ürün ismi 10-60 karakter arasında olmalıdır.")]
        public string Name { get; set; }

        //[Required(ErrorMessage = "Image Url alanı boş geçilemez")]
        [Display(Name = "Image Url")]
        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Description alanı boş geçilemez")]
        [StringLength(10000, MinimumLength = 20, ErrorMessage = "Ürün açıklaması 20-10000 karakter arasında olmalıdır.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Fiyat alanı boş geçilemez")]
        [Range(1, 10000)]
        public decimal? Price { get; set; }

        public List<Category> SelectedCategories { get; set; }
    }
}
