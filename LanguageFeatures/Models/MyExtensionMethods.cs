using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LanguageFeatures.Models
{
    public static class MyExtensionMethods
    {
        public static decimal TotalPrices(this IEnumerable<Product> productParam) {
            // this keyword infront of fist parameter marks TotalPrices as
            // an extension method
            decimal total = 0;
            foreach (Product prod in productParam)
            {
                total += prod.Price;
            }
            return total;
        }

        public static IEnumerable<Product> FilterByCategory(
                this IEnumerable<Product> productEnum, string categoryParam) {
            foreach (Product prod in productEnum) {
                if (prod.Category == categoryParam) {
                    yield return prod;
                }
            }
        }

        // More general filter with delegate (Func) against each Product
        public static IEnumerable<Product> Filter(
                this IEnumerable<Product> productEnum, Func<Product, bool> selectorParam) {
            foreach (Product prod in productEnum) {
                if (selectorParam(prod)) {
                    yield return prod;
                }
            }
        }
    }
}
