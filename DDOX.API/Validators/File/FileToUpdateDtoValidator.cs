using DDOX.API.Dtos.File;
using DDOX.API.Dtos.FileIndicesCore;
using DDOX.API.Repository.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Validators.File
{
    public class FileToUpdateDtoValidator : AbstractValidator<FileToUpdateDto>
    {
        public FileToUpdateDtoValidator(DataContext context)
        {
            RuleFor(f => f.Name).NotEmpty();
            RuleFor(f => f.UpdatedBy).NotEmpty();

            RuleForEach(f => f.FileIndices)
                .ChildRules(f => f.RuleFor(i => i.IndexId).NotEmpty())
                .ChildRules(f => f.RuleFor(i => i.Value).NotEmpty())
                .SetValidator(new IndexValidator<FileToUpdateDto, FileIndicesDto>(context))
                .When(f => f.FileIndices != null);
        }
    }
}
