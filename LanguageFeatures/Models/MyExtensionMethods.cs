﻿using System;
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
    }
}
