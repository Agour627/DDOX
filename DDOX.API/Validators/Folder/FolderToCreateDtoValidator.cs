using DDOX.API.Dtos.Folder;
using DDOX.API.Dtos.FolderIndices;
using DDOX.API.Repository.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Validators.Folder
{
    public class FolderToCreateDtoValidator : AbstractValidator<FolderToCreateDto>
    {
        public FolderToCreateDtoValidator(DataContext context)
        {
            RuleFor(f => f.Name).NotEmpty();
            RuleFor(f => f.PartitionId).NotEmpty();
            RuleFor(f => f.FolderType).NotEmpty();
            RuleFor(f => f.CreatedBy).NotEmpty();

            RuleForEach(f => f.FolderIndices)
                .ChildRules(f => f.RuleFor(i => i.IndexId).NotEmpty())
                .ChildRules(f => f.RuleFor(i => i.Value).NotEmpty())
                .SetValidator(new IndexValidator<FolderToCreateDto, FolderIndicesToCreateDto>(context))
                .When(f => f.FolderIndices != null);
            
            RuleForEach(f => f.FolderCategories)
                .ChildRules(f => f.RuleFor(c => c.CategoryId).NotEmpty())
                .When(f => f.FolderCategories != null);
        }
    }
}
