using AutoMapper;
using Caretta.Business.Abstract;
using Caretta.Core.Entity;
using Caretta.Data.Context;
using Microsoft.EntityFrameworkCore;
using Caretta.Core.Exceptions;
using Caretta.Core.Dto.CategoryDto;
using Microsoft.Data.SqlClient.Server;
using Caretta.Core.Enums;


namespace Caretta.Business.Concrete
{


    public class CategoryService : ICategoryInterface
    {

        public readonly CarettaContext contextCategory;
        public readonly IMapper _mapper;
        private readonly UserContextService _userContextService;
        private readonly IContentInterface _contentService;
        public CategoryService(CarettaContext context, IMapper mapper, UserContextService userContextService, IContentInterface contentService)
        {
            contextCategory = context;
            _mapper = mapper;
            _userContextService = userContextService;
            _contentService = contentService;
        }
        /*
        public async Task<IEnumerable<CategoryGetDto>> GetAllCategoriesAsync()
        {
            var categories = await contextCategory.Categories
                .Include(c => c.ContentCategories)
                    .ThenInclude(cc => cc.Content)
                    .Where(c => c.IsDeleted == false)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CategoryGetDto>>(categories);
        }
        */
        
        public async Task<CategoryGetByIdDto> GetCategoryByIdAsync(Guid categoryId)
        {
            
            try { 
            var category = await contextCategory.Categories
                .Where(c => c.Id == categoryId)
                .Include(c => c.ContentCategories)
                    .ThenInclude(cc => cc.Content)
                .FirstOrDefaultAsync();

                if (category == null)
            {
                throw new NotFoundException("Category not found.");
            }
                if (categoryId == Guid.Empty)
                {
                    throw new InvalidIdException("Invalid category ID.");
                }
                if (category.IsDeleted)
            {
                throw new DeletedException("Category is deleted.");
            }
                category.ContentCategories = category.ContentCategories
            .Where(cc => cc.Content.ContentStatus == ContentStatus.Published && cc.Content.IsApproved)
            .ToList();


                return _mapper.Map<CategoryGetByIdDto>(category);
            }
            catch (NotFoundException ex)
            {
                throw new Exception("Category not found", ex);
            }
            catch (DeletedException ex)
            {
                throw new Exception("Category is deleted", ex);
            }
            catch (Exception ex) 
            {
                throw new Exception("500 exception", ex);
            }
        }
        /*
        public async Task<CategoryCreateDto> CreateOrUpdateCategoryAsync(Guid? categoryId, CategoryCreateDto categoryDto)
        {
            Category category;

            if (categoryId.HasValue)
            {
                
                category = await contextCategory.Categories
                    .Include(c => c.ContentCategories)
                        .ThenInclude(cc => cc.Content)
                    .FirstOrDefaultAsync(c => c.Id == categoryId.Value);

                if (category == null)
                {
                    throw new NotFoundException("Category to be updated not found.");
                }

                if (category.IsDeleted)
                {
                    throw new DeletedException("Category to be updated is deleted.");
                }

                _mapper.Map(categoryDto, category);
                category.ModifiedOn = DateTime.UtcNow;
                category.ModifiedBy = _userContextService.GetCurrentUserId();
                contextCategory.Categories.Update(category);
            }
            else
            {

                category = _mapper.Map<Category>(categoryDto);
                category.CreatedOn = DateTime.UtcNow;
                category.CreatedBy = _userContextService.GetCurrentUserId();
                contextCategory.Categories.Add(category);
            }

            


            await contextCategory.SaveChangesAsync();

            return _mapper.Map<CategoryCreateDto>(category);
        }

        */







        //is deleted soft delete
        
        public async Task DeleteCategoryAsync(Guid categoryId)
        {
            var category = await contextCategory.Categories
                .Include(c => c.ContentCategories)
                    .ThenInclude(cc => cc.Content)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
            {
                throw new NotFoundException("Category to be deleted not found.");
            }
            if (category.IsDeleted)
            {
                throw new DeletedException("Category to be deleted is already deleted.");
            }

            category.IsDeleted = true;
            contextCategory.Categories.Update(category);

            // Check related contents
            foreach (var contentCategory in category.ContentCategories)
            {
                var content = contentCategory.Content;
                if (content.ContentCategories.Count == 1)
                {
                    await _contentService.DeleteContentAsync(content.Id);
                    //content.IsDeleted = true;
                    //contextCategory.Contents.Update(content);
                }
                else
                {
                    content.ContentCategories.Remove(contentCategory);
                    contextCategory.ContentCategories.Remove(contentCategory);
                }
            }

            await contextCategory.SaveChangesAsync();
        }


        public async Task AddFavouriteCategoryAsync( Guid categoryId)
        {
            


            var category = await contextCategory.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null)
                throw new NotFoundException("Category not found.");

            if (categoryId != null)
            {
                //foreach (categoryId)
                {
                    var userFavouriteCategories = new UserFavouriteCategories
                    {
                        UserId = _userContextService.GetCurrentUserId(),
                        CategoryId = categoryId
                    };
                    contextCategory.UserFavouriteCategories.Add(userFavouriteCategories);
                }
            }

            await contextCategory.SaveChangesAsync();
        }
        

    }

}
