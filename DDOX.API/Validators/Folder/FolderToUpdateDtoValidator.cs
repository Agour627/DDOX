using DDOX.API.Dtos.Folder;
using DDOX.API.Repository.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Validators.Folder
{
    public class FolderToUpdateDtoValidator : AbstractValidator<FolderToUpdateDto>
    {
        public FolderToUpdateDtoValidator(DataContext context)
        {
            RuleFor(f => f.Name).NotEmpty();
            RuleFor(f => f.UpdatedBy).NotEmpty();

            RuleForEach(f => f.FolderIndices)
                .ChildRules(f => f.RuleFor(i => i.IndexId).NotEmpty())
                .ChildRules(f => f.RuleFor(i => i.Value).NotEmpty())
                .SetValidator(new IndexValidator<FolderToUpdateDto, FolderIndicesDto>(context))
                .When(f => f.FolderIndices != null);
            
            RuleForEach(f => f.FolderCategories)
                .ChildRules(f => f.RuleFor(c => c.CategoryId).NotEmpty())
                .When(f => f.FolderCategories != null);
        }
    }
}
