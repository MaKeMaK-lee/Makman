using Makman.Middle.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makman.Middle.EntityManagementServices
{
    public interface ITagManagementService
    { 
        void AddNew(string name);
        Tag? Create(string name);
        /// <summary>
        /// 
        /// </summary> 
        /// <returns>True if all tags category is same (some or all may be null) or if tags is null or empty, otherwise false </returns>
        bool IsSameOrNullCategory(IEnumerable<Tag>? tags);
        void Remove(IEnumerable<Tag> tags);
        void SetCategory(IEnumerable<Tag> tags, TagCategory? tagCategory);
    }
}
