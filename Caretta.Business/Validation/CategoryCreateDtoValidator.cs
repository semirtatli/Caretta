using Caretta.Core.Dto.CategoryDto;
using FluentValidation;

namespace Caretta.Business.Validation
{

    public class CategoryCreateDtoValidator : AbstractValidator<CategoryCreateDto>
    {
        public CategoryCreateDtoValidator()
        {
            //
            RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .Length(1, 30).WithMessage("Category name must be between 1 and 30 characters.")
            .Matches("^[a-zA-Z ]*$").WithMessage("Category name must not contain numbers or special characters.");

            /*
                        RuleFor(x => x.ContentCategories)
                                .NotEmpty().WithMessage("There must be at least one ContentCategory.");
            */
        }
    }

}