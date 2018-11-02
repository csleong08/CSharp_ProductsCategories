using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;    

namespace ProductsCategories.Models
{
    public class Users
    {
        [Key]
        public int id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public DateTime updated_at { get; set; }
        public DateTime created_at { get; set; }
        public Users()
        {
            Product = new List<Products>();
            Category = new List<Categories>();
        }
        public List<Products> Product { get; set;}
        public List<Categories> Category { get; set;}
    }
}