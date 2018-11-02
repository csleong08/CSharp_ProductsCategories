using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsCategories.Models
{
    public class Categories
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime created_at { get; set; }
        public int usersid { get; set; }
        public Users Users { get; set; }
        public Categories()
        {
            CategoriesProduct = new List<CategoriesProducts>();
        }
        public List<CategoriesProducts> CategoriesProduct { get; set;}
    }
}