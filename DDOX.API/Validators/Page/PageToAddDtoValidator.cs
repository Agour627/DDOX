using DDOX.API.Dtos.Page;
using DDOX.API.Dtos.PageIndices;
using DDOX.API.Repository.Data;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Validators.Page
{
    public class PageToAddDtoValidator : AbstractValidator<PageToAddDto>
    {
        public PageToAddDtoValidator(DataContext context)
        {
            RuleFor(p => p.PageNumber).NotEmpty();
            RuleFor(p => p.FileId).NotEmpty();
            RuleForEach(p => p.PageIndices)
                .ChildRules(f => f.RuleFor(i => i.IndexId).NotEmpty())
                .ChildRules(f => f.RuleFor(i => i.Value).NotEmpty())
                .SetValidator(new IndexValidator<PageToAddDto, PageIndicesToAddDto>(context))
                .When(p => p.PageIndices != null);

        }
    }
}
