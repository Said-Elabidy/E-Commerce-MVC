using BusinessLayer.DTO.Product;
using BusinessLayer.Manager.IManager;
using E_Commerce_MVC.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_MVC.Controllers
{

    public class CartController : Controller
    {
        private readonly IProductManager _productManager;



        public CartController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        public IActionResult ShowCart()
        {
            var cart = HttpContext.Session.Get<List<ProductCardDTO>>("Cart") ?? new List<ProductCardDTO>();
            return View("CartItems",cart);
        }
        [Authorize]
        [HttpGet]
        // Action to add a product to the cart
        public async Task<IActionResult> AddToCart(int productId)
        {
            var product = await _productManager.GetProductByIdAsync(productId);

            if (product == null)
            {
                return NotFound();  
            }

            var cart = HttpContext.Session.Get<List<ProductCardDTO>>("Cart") ?? new List<ProductCardDTO>();

            // Add the product to the cart
            cart.Add(product);

            HttpContext.Session.Set("Cart", cart);

            return RedirectToAction("Index", "Home");
            //return Json(new { success = true, message = "Product added to cart successfully." });
        }
    }
}

