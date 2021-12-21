using DDOX.API.Dtos.Category;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Validators.Category
{
    public class CategoryToCreateDtoValidator : AbstractValidator<CategoryToCreateDto> 
    {
        public CategoryToCreateDtoValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.PartitionId).NotEmpty();
            RuleFor(c => c.Type).NotEmpty();
            RuleFor(c => c.CreatedBy).NotEmpty();
            RuleForEach(c => c.CategoryIndices)
                .ChildRules(c => c.RuleFor(i => i.IndexId).NotEmpty())
                .ChildRules(c => c.RuleFor(i => i.IsRequried).NotEmpty())
                .When(c => c.CategoryIndices != null);

        }
    }
}
