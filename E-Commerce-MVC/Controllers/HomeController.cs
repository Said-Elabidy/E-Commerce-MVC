using System.Diagnostics;
using BusinessLayer.Manager.IManager;
using BusinessLayer.DTO.Category;
using E_Commerce_MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_MVC.Controllers
{

    public class HomeController : Controller
    {
        private readonly ICategoryWithProductManager _categoryWithProductManager;

        public HomeController(ICategoryWithProductManager categoryWithProductManager)
        {
            _categoryWithProductManager = categoryWithProductManager;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var CategoryWithProducts =await _categoryWithProductManager.GetAllCategoryWithProducts();
            return View("Index", CategoryWithProducts);
        }

        [HttpGet]
        public IActionResult ShowAllProducts()
        {

            return View("ShowProducts");
        }
    }

}
