using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LanguageFeatures.Models;
using System.Text;

namespace LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public string Index()
        {
            return "Navigate to a URL to show an example";
        }

        public ViewResult AutoProperty() {
            // create a new Product object
            Product myProduct = new Product();

            //set the property value 
            myProduct.Name = "Kayak";

            // get property
            string productName = myProduct.Name;

            // generate the view
            // View method overload for parameters (string, object) v. (string, string)
            // are different - the explicit cast is to avoid misuse 
            return View("Result", 
                (object)String.Format("Product Name: {0}", productName)); 
        }

        public ViewResult CreateProduct() {
            // create new Product object
            Product myProduct = new Product {
                ProductID = 100, Name = "Kayak",
                Description = "A boat for one person",
                Price = 275M, Category = "Watersports"
            };

            return View("Result", 
                (object)String.Format("Product Name: {0}", myProduct.Category));
        }

        public ViewResult CreateCollection() {
            string[] stringArray = { "apple", "orange", "plum" };

            List<int> intList = new List<int> { };

            Dictionary<string, int> myDict = new Dictionary<string, int>
            {
                {"apple", 10}, {"orange", 10}, {"plum", 30}
            };

            return View("Result", (object)stringArray[1]);
        }

        public ViewResult UseExtension() {
            // create and populate ShoppingCart
            ShoppingCart cart = new ShoppingCart
            {
                Products = new List<Product> {
                    new Product { Name = "Kayak", Price = 275M},
                    new Product { Name = "Lifejacket", Price = 48.95M},
                    new Product { Name = "Soccer ball", Price = 19.50M},
                    new Product { Name = "Corner flag", Price = 34.95M}
                }
            };

            // get total value in the cart
            decimal cartTotal = cart.TotalPrices();

            return View("Result", 
                (object)String.Format("Total: {0:c}", cartTotal));
        }

        public ViewResult UseExtensionEnumerable() {
            IEnumerable<Product> products = new ShoppingCart
            {
                Products = new List<Product> {
                    new Product { Name = "Kayak", Price = 275M },
                    new Product { Name = "Lifejacket", Price = 48.95M },
                    new Product { Name = "Soccer ball", Price = 19.50M },
                    new Product { Name = "Corner flag", Price = 34.95M }
                }
            };

            // create and populate an array of Products objects
            Product[] productArray = {
                new Product { Name = "Canoe", Price = 150M },
                new Product { Name = "Paddle", Price = 16.00M },
                new Product { Name = "Tea Cup Coaster Set", Price = 25.00M },
                new Product { Name = "Gumption", Price = 100M }
            };

            decimal cartTotal = products.TotalPrices();
            decimal arrayTotal = productArray.TotalPrices();

            return View("Result", (object)String.Format("Cart Total: {0}, Array Total: {1}", cartTotal, arrayTotal));
        }

        public ViewResult UseFilterExtensionMethod() {
            IEnumerable<Product> products = new ShoppingCart
            {
                Products = new List<Product> {
                    new Product { Name = "Kayak", Category = "Watersports", Price = 275M },
                    new Product { Name = "Lifejacket", Category = "Watersports", Price = 48.95M },
                    new Product { Name = "Soccer ball", Category = "Soccer", Price = 19.50M },
                    new Product { Name = "Corner flag", Category = "Soccer", Price = 34.95M }
                }
            };
            string category = "Soccer";
            // define filter criteria via delegate / Func
            /*
            Func<Product, bool> categoryFilter = delegate (Product prod)
            {
                return prod.Category == "Soccer";
            };
            */
            // Lambda expression
            // Func<Product, bool> categoryFilter = prod => prod.Category == "Soccer";

            decimal total = 0;
            foreach (Product prod in products.Filter(prod => prod.Category == "Soccer" && prod.Price > 20))
            {
                total += prod.Price;
            }

            return View("Result", (object)String.Format("Category: {0}; Total: {1}", category, total));
        }

        public ViewResult FindTopThreeExpensiveProducts()
        {
            Product[] products=  {
                    new Product { Name = "Kayak", Category = "Watersports", Price = 275M },
                    new Product { Name = "Lifejacket", Category = "Watersports", Price = 48.95M },
                    new Product { Name = "Soccer ball", Category = "Soccer", Price = 19.50M },
                    new Product { Name = "Corner flag", Category = "Soccer", Price = 34.95M }
                };
            // Use LINQ query syntax
            //var descendingProducts = from prod in products
            //                         orderby prod.Price descending
            //                         select new { prod.Name, prod.Price };

            // Use LINQ Dot notation
            var descendingProducts = products.OrderByDescending(e => e.Price)
                                        .Take(3)
                                        .Select(e => new { e.Name, e.Price});

            // Use deferred LINQ extension Methods
            products[3] = new Product { Name = "Gumption", Price = 100M }; 

            // int count = 0;
            StringBuilder result = new StringBuilder();
            // query is NOT evaluate unitl the results are enumerated
            // everytime results are enumerated the result reflect the current state of source data
            foreach (var p in descendingProducts) {
                result.AppendFormat("Price: {0} ", p.Price);
                // if (++count == 3)
                // {
                    //break;
                //}
            }
            return View("Result", (object)result.ToString());
        }

        public ViewResult SumOfProducts()
        {
            Product[] products =  {
                    new Product { Name = "Kayak", Category = "Watersports", Price = 275M },
                    new Product { Name = "Lifejacket", Category = "Watersports", Price = 48.95M },
                    new Product { Name = "Soccer ball", Category = "Soccer", Price = 19.50M },
                    new Product { Name = "Corner flag", Category = "Soccer", Price = 34.95M }
                };

            var results = products.Sum(e => e.Price);

            products[3] = new Product { Name = "MissedItem", Price = 10000M };

            return View("Result", (object)String.Format("Sum: {0:c}", results));
        }

        // Anonymously typed objects
        public ViewResult CreateAnonArray()
        {
            var solidArray = new[]
            {
                new { Initial = 'S', Principle = "Single Responsibility Principle" },
                new { Initial = 'O', Principle = "Open / Close Principle"},
                new { Initial = 'L', Principle = "Liskov Substitution Principle"},
                new { Initial = 'I', Principle = "Interface Segregation Principle"},
                new { Initial = 'D', Principle = "Dependency Inversion Principle"} 
            };
            StringBuilder solidPrinciples = new StringBuilder();
            foreach (var item in solidArray)
            {
                solidPrinciples.AppendLine(item.Initial.ToString()).Append(": ").Append(item.Principle).Append("; ");
            }
            
            return View("Result", (object)solidPrinciples.ToString());
        }
    }
}