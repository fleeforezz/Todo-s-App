using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.DAL.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly TodoDbContext _db;

        public CategoryRepository(TodoDbContext db)
        {
            _db = db;
        }

        /*
        *  Create
        */
        public Category Create(Category category)
        {
            _db.Add(category);
            _db.SaveChanges();
            return category;
        }

        /*
        *  Delete
        */
        public bool Delete(Category category)
        {
            _db.Remove(category);
            _db.SaveChanges();
            return true;
        }

        /*
        *  Delete All
        */
        public void DeleteAll()
        {
            throw new NotImplementedException();
        }

        /*
        *  Get All
        */
        public List<Category> GetAll()
        {
            return _db.Categories.ToList();
        }

        /*
        *  Get By Id
        */
        public Category? GetById(Guid categoryId)
        {
            return _db.Categories.FirstOrDefault(c => c.CategoryId == categoryId);
        }

        /*
        *  Get By User Id
        */
        public List<Category> GetByUserId(Guid userId)
        {
            return _db.Categories
                      .Where(c => c.UserId == userId)
                      .ToList();
        }

        /*
        *  Update
        */
        public bool Update(Category category)
        {
            _db.Update(category);
            _db.SaveChanges();
            return true;
        }
    }
}
