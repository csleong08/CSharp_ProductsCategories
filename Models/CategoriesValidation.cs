using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProductsCategories.Models
{
    public class CategoriesValidation
    {
        [Required(ErrorMessage = "Name is required to create category")]
        public string name { get; set; }
    }
}