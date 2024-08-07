using Caretta.Core.Entity;
using Caretta.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.Abstract
{
    public interface IContentCategoriesInterface
    {
        Task<IEnumerable<ContentCategoryDto>> GetAllContentCategoriesAsync();
        Task AddContentCategoryAsync(ContentCategoryDto contentCategories);
        Task RemoveContentCategoryAsync(Guid contentId, Guid categoryId);
    }
}
