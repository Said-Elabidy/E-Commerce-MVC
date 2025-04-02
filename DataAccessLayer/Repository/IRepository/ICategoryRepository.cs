using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repository.IRepository
{
    public interface ICategoryRepository
    {
        public Task<List<Category>> GetCategoriesAsync();

        public Task<List<Category>> GetCategoriesWithProductsAsync();

    }
}
