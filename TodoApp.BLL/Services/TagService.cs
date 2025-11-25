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
    public class TagService : ITagService
    {
        private readonly TagRepository _repo;

        public TagService(TagRepository repo)
        {
            _repo = repo;
        }

        /*
        *  Create new Category
        */
        public Tag CreateTag(Tag tag)
        {
            if (string.IsNullOrWhiteSpace(tag.TagName))
            {
                throw new ArgumentNullException("Tag name cannot be empty");
            }

            var newTag = new Tag()
            {
                TagId = Guid.NewGuid(),
                UserId = tag.UserId,
                TagName = tag.TagName,
                CreatedAt = tag.CreatedAt,
            };

            return _repo.Create(newTag);
        }

        /*
        *  Delete Category
        */
        public bool DeleteTag(Tag tag)
        {
            var existingTag = _repo.GetById(tag.TagId);
            if (existingTag != null)
            {
                return _repo.Delete(existingTag);
            }
            else { return false; }
        }

        /*
        *  Get All Categories
        */
        public List<Tag> GetTags(Guid userId)
        {
            return _repo.GetByUserId(userId);
        }

        /*
        *  Get All Categories by Id
        */
        public Tag? GetTagById(Guid tagId)
        {
            return _repo.GetById(tagId);
        }

        /*
        *  Update Category
        */
        public bool UpdateTag(Tag tag)
        {
            if (string.IsNullOrWhiteSpace(tag.TagName))
            {
                throw new ArgumentNullException("Tag name cannot be empty");
            }

            return _repo.Update(tag);
        }
    }
}
