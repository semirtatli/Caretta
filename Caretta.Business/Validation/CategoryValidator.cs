using Caretta.Core.Entity;
using FluentValidation;

namespace Caretta.Business.Validation
{
    
    public class CategoryValidator : AbstractValidator<Category>
    {
        public CategoryValidator() 
        {
            RuleFor(x => x.Id)
                   .NotEmpty().WithMessage("ID is required.");
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
