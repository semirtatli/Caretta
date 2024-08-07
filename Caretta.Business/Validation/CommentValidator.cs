using Caretta.Core.Dto.CommentDto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caretta.Business.Validation
{
    public class CommentValidator : AbstractValidator<CommentCreateDto>
    {
        public CommentValidator() 
        {
            RuleFor(x => x.ContentId)
            .NotEmpty().WithMessage("Content ID is required.");

//
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Text is required.")
                .Length(1, 300).WithMessage("Text must be between 1 and 300 characters.");
            //
        }
    }
}
