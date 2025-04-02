using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetProductByIdAsync(int Id)
        {
            var Product = await _context.Products.FirstOrDefaultAsync(x => x.Id == Id);

            return Product ?? throw new KeyNotFoundException($"Product with ID {Id} not found."); 
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var Products = await _context.Products.ToListAsync();

            return Products;
        }

        public async Task UpdateProductAsync(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            _context.Products.Update(product);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Product product)
        {
            if (product == null) throw new ArgumentNullException(nameof(product));

            _context.Products.Remove(product);

            await _context.SaveChangesAsync();
        }

        public async Task CreateProductAsync(Product product)
        {
            if(product == null) throw new ArgumentNullException( nameof(product));

           await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();
        }


        public async Task<List<Product>> GetProductByCategoryNameAsync(string categoryName)
        {
            if(categoryName==null || categoryName==string.Empty)
            {
                throw new ArgumentNullException(nameof(categoryName));
            }

            var Category = await  _context.Categories.Include(p =>p.Products).FirstOrDefaultAsync(c => c.Name == categoryName);

            if (Category == null)
            {
                throw new ArgumentNullException(nameof(Category));
            }

            var ProductsByCategory = await _context.Products.Where(p => p.CategoryId == Category.Id).ToListAsync();

            return ProductsByCategory;


        }

        public async Task DeleteProductAsync(int Id)
        {
            var product =await GetProductByIdAsync(Id);
             _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }
    }
}
