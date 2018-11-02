using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsCategories.Models
{
    public class CategoriesProducts
    {
        [Key]
        public int id { get; set; }
        public int categoriesid { get; set; }
        public Categories Categories { get; set; }
        public int productsid { get; set; }
        public Products Products { get; set; }
    }
}