using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.BLL.Interfaces
{
    public interface ICategoryService
    {
        Category CreateCategory(Category category);
        List<Category> GetCategories(Guid userId);
        Category GetCategoryById(Guid categoryId);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
    }
}
