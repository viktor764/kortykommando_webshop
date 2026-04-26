using System.Collections.Generic;
using Hotcakes.Commerce.Catalog;

namespace PoharnokProject.Dnn.Dnn.PoharnokProject.Cocktail.Models
{
    public class IngredientCategoryViewModel
    {
        public string CategoryName { get; set; }

        // A Hotcakes saját Product osztályát használjuk a lista elemeihez
        public List<Product> Products { get; set; }
    }
}