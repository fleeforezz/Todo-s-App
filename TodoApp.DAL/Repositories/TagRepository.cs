using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.DAL.Repositories
{
    public class TagRepository : IRepository<Tag>
    {
        private readonly TodoDbContext _db;

        public TagRepository(TodoDbContext db)
        {
            _db = db;
        }

        /*
        *  Create
        */
        public Tag Create(Tag category)
        {
            _db.Add(category);
            _db.SaveChanges();
            return category;
        }

        /*
        *  Delete
        */
        public bool Delete(Tag category)
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
        public List<Tag> GetAll()
        {
            return _db.Tags.ToList();
        }

        /*
        *  Get By Id
        */
        public Tag? GetById(Guid tagId)
        {
            return _db.Tags.FirstOrDefault(c => c.TagId == tagId);
        }

        /*
        *  Get By User Id
        */
        public List<Tag> GetByUserId(Guid userId)
        {
            return _db.Tags
                      .Where(c => c.UserId == userId)
                      .ToList();
        }

        /*
        *  Update
        */
        public bool Update(Tag category)
        {
            _db.Update(category);
            _db.SaveChanges();
            return true;
        }
    }
}
