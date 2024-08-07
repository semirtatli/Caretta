using AutoMapper;
using Caretta.Business.Abstract;
using Caretta.Core.Dto;
using Caretta.Core.Entity;
using Caretta.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Caretta.Business.Concrete
{
    public class ContentCategoriesService : IContentCategoriesInterface
    {
        private readonly CarettaContext contextContentCategories;
        private readonly IMapper _mapper;

        public ContentCategoriesService(CarettaContext context, IMapper mapper)
        {
            contextContentCategories = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ContentCategoryDto>> GetAllContentCategoriesAsync()
        {
            var contentCategories = await contextContentCategories.ContentCategories.ToListAsync();
            return _mapper.Map<IEnumerable<ContentCategoryDto>>(contentCategories);
        }

        public async Task AddContentCategoryAsync(ContentCategoryDto contentCategoryDto)
        {
            var contentCategory = _mapper.Map<ContentCategories>(contentCategoryDto);
            contextContentCategories.ContentCategories.Add(contentCategory);
            await contextContentCategories.SaveChangesAsync();
        }

        public async Task RemoveContentCategoryAsync(Guid contentId, Guid categoryId)
        {
            var contentCategory = await contextContentCategories.ContentCategories
                .FirstOrDefaultAsync(cc => cc.ContentId == contentId && cc.CategoryId == categoryId);

            if (contentCategory != null)
            {
                contextContentCategories.ContentCategories.Remove(contentCategory);
                await contextContentCategories.SaveChangesAsync();
            }
        }
    }
}
