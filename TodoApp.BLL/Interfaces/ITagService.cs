using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoApp.DAL.Entities;

namespace TodoApp.BLL.Interfaces
{
    public interface ITagService
    {
        Tag CreateTag(Tag tag);
        List<Tag> GetTags(Guid userId);
        Tag GetTagById(Guid tagId);
        bool UpdateTag(Tag tag);
        bool DeleteTag(Tag tag);
    }
}
