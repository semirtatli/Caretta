using Caretta.Core.Dto.CategoryDto;
using Caretta.Core.Entity;

namespace Caretta.Business.Abstract
{

    public interface ICategoryInterface
    {

        //Task<IEnumerable<CategoryGetDto>> GetAllCategoriesAsync();
        Task<CategoryGetByIdDto> GetCategoryByIdAsync(Guid categoryId);
        //Task<CategoryCreateDto> CreateOrUpdateCategoryAsync(Guid? categoryId, CategoryCreateDto categoryCreateDto);
        Task DeleteCategoryAsync(Guid categoryId);
        Task AddFavouriteCategoryAsync(Guid categoryId);

    }

}