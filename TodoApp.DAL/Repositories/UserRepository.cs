using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.DAL.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly TodoDbContext _db;

        public UserRepository(TodoDbContext db)
        {
            _db = db;
        }

        /*
        *  Create  
        */
        public User Create(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
            return user;
        }

        /*
        *  Delete  
        */
        public bool Delete(User user)
        {
            _db.Remove(user);
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
        public List<User> GetAll()
        {
            return _db.Users.ToList();
        }

        public User? GetById(Guid id)
        {
            return _db.Users.FirstOrDefault(x => x.UserId == id);
        }

        /*
        *  Get By Email  
        */
        public User? GetByEmail(string email)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email);
        }

        /*
        *  Get By Email/Password
        */
        public User? GetByEmailPassword(string email, string password)
        {
            return _db.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == password);
        }

        /*
        *  Update 
        */
        public bool Update(User user)
        {
            _db.Users.Update(user);
            _db.SaveChanges();
            return true;
        }
    }
}
