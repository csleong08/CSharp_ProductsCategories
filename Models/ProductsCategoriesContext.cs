using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProductsCategories.Models
{
    public class ProductsCategoriesContext : DbContext
    {
        internal IEnumerable<object> productscategories;

        public DbSet<Users> users { get; set; } // always make users lowercase
        public DbSet<Products> products { get; set; } // always make users lowercase
        public DbSet<Categories> categories { get; set; } // always make users lowercase
        public DbSet<CategoriesProducts> categoriesproducts { get; set; } // always make users lowercase

        // base() calls the parent class' constructor passing the "options" parameter along
        public ProductsCategoriesContext(DbContextOptions<ProductsCategoriesContext> options) : base(options) { }
    }
}