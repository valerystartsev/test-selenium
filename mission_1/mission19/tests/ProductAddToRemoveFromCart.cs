using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace mission19
{
    [TestFixture]
    public class ProductsAddToRemoveFromCartTests : TestBase
    {
        [Test]
        public void ProductsAddToRemoveFromCart()
        {

            // get href links for most popular products 
            var products = app.GetMostPopularProductLinks();

            Console.WriteLine(products.Count );
            
            // add any 3 product to Cart: first, last and middle
            // GetProduct().Open(href_link) opens product page by passed link
            // ClickAddtoCart() clicks Add To Cart on product page
            app.GetProduct().Open(products.First() ).ClickAddToCart();
            app.GetProduct().Open(products.Last() ).ClickAddToCart();
            app.GetProduct().Open(products.ElementAt(products.Count/2 + 1)).ClickAddToCart();

            // GetCartPage returns Cart page
            // RemoveAllItemsFromCart() step by step delete all products ftom the cart
            app.GetCartPage().RemoveAllItemsFromCart();
            
        }
    }
}