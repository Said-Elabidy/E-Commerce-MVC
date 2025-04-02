﻿using BusinessLayer.DTO.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Manager.IManager
{
    public interface ICategoryWithProductManager
    {
        public Task<List<CategoryWithProductDTO>> GetAllCategoryWithProducts();
    }
}
