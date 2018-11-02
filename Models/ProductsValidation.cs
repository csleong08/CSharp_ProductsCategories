using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsCategories.Models
{
    public class ProductsValidation
    {
        [Required(ErrorMessage = "Name is required to create product")]
        public string name { get; set; }
        [Required(ErrorMessage = "Description is required to create product")]
        public string description { get; set; }
        [Required(ErrorMessage = "Price is required to create product")]
        public double price{ get; set; }
    }
}