using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.BLL.Interfaces;
using TodoApp.DAL.Entities;
using TodoApp.DAL.Repositories;

namespace TodoApp.BLL.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly CategoryRepository _repo;

        public CategoryService(CategoryRepository repo)
        {
            _repo = repo;
        }

        /*
        *  Create new Category
        */
        public Category CreateCategory(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentNullException("Category name cannot be empty");
            }

            var newCategory = new Category()
            {
                CategoryId = Guid.NewGuid(),
                UserId = category.UserId,
                Name = category.Name,
                CreatedAt = category.CreatedAt,
            };

            return _repo.Create(newCategory);
        }

        /*
        *  Delete Category
        */
        public bool DeleteCategory(Category category)
        {
            var existingCategory = _repo.GetById(category.CategoryId);
            if (existingCategory != null)
            {
                return _repo.Delete(existingCategory);
            }
            else { return false; }
        }

        /*
        *  Get All Categories
        */
        public List<Category> GetCategories(Guid userId)
        {
            return _repo.GetByUserId(userId);
        }

        /*
        *  Get All Categories by Id
        */
        public Category? GetCategoryById(Guid categoryId)
        {
            return _repo.GetById(categoryId);
        }

        /*
        *  Update Category
        */
        public bool UpdateCategory(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name))
            {
                throw new ArgumentNullException("Category name cannot be empty");
            }

            return _repo.Update(category);
        }
    }
}
