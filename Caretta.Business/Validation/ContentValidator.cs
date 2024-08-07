using Caretta.Core.Entity;
using FluentValidation;
using FluentValidation.Validators;
using System.ComponentModel.DataAnnotations;

namespace Caretta.Business.Validation
{
    public class ContentValidator : AbstractValidator<Content>
    {
        
            public ContentValidator()
            {
                RuleFor(x => x.Id)
                   .NotEmpty().WithMessage("ID is required.");

            //

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .Length(1, 50).WithMessage("Title must be between 1 and 50 characters.");
                    //.Matches("^[a-zA-Z ]*$").WithMessage("Content Title must not contain numbers or special characters.");

            RuleFor(x => x.Body)
                    .NotEmpty().WithMessage("Body is required.")
                    .Length(1, 350).WithMessage("Body must be between 1 and 350 characters.");
            //
/*
                RuleFor(x => x.ContentCategories)
                    .NotEmpty().WithMessage("There must be at least one ContentCategory.");
            
                RuleFor(x => x.Comments)
                    .NotEmpty().WithMessage("There must be at least one Comment.");
*/
        }
        

    }
}
