using BusinessLayer.DTO;
using BusinessLayer.DTO.Product;
using BusinessLayer.Manager;
using BusinessLayer.Manager.IManager;
using BusinessLayer.Services;
using BusinessLayer.Services.IServices;
using DataAccessLayer.Entities;
using DataAccessLayer.Repository.IRepository;
using E_Commerce_MVC.ActionRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce_MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IFileServices _fileServices;
        private readonly IProductManager _productManager;

        public ProductController(IFileServices fileServices, IProductManager productManager)
        {
            _fileServices = fileServices;
            _productManager = productManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        // GET: Product/AddProduct
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            var categories = await _productManager.GetAllCategories();
            ViewBag.Categories = categories;
            return View();
        }


        // POST: Product/Create
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductActionRequest ProductFromReq)
        {
            if (ProductFromReq.ImagePath == null)
            {
                ModelState.AddModelError("ImagePath", "Please upload an image.");
            }

            if (!ModelState.IsValid)
            {
                // Log or inspect ModelState errors
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return View(ProductFromReq);
            }
            if (ModelState.IsValid)
            {
                var UniqueFileName = _fileServices.UploadFile(ProductFromReq.ImagePath, "Product/");

                // Mapping From ProductFromRequest to Product DTO
                var ProductDTO = new CreateProductDTO
                {
                    Name = ProductFromReq.Name,
                    Description = ProductFromReq.Description,
                    Price = ProductFromReq.Price,
                    ImagePath = UniqueFileName,
                    CategoryId = ProductFromReq.CategoryId
                };

                await _productManager.AddProduct(ProductDTO);


            }

            return View("AddProduct");
        }
     

        [HttpGet]
        [Route("{CategoryName}")]
        public async Task<IActionResult> GetProductByCategoryName(string CategoryName)
        {
            if (CategoryName == null || CategoryName == string.Empty)
            {
                throw new ArgumentNullException(nameof(CategoryName));
            }

            var Products =await _productManager.GetAllProductByCategoryName(CategoryName);

            return View("ShowProducts",Products);
        }

        [HttpGet]
        public async Task<IActionResult> Delete (int ProductId)
        {   
          await  _productManager.Delete(ProductId);
          return RedirectToAction("Index","Home");
        }

    }
}
