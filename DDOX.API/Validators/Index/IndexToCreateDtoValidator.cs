using DDOX.API.Dtos.Index;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Validators.Index
{
    public class IndexToCreateDtoValidator : AbstractValidator<IndexToCreateDto>
    {
        public IndexToCreateDtoValidator()
        {
            RuleFor(i => i.Label).NotEmpty();
            RuleFor(i => i.PartitionId).NotEmpty();
            RuleFor(i => i.Type).NotEmpty();
            RuleFor(i => i.CreatedBy).NotEmpty();
            RuleForEach(i => i.IndexRestrictions)
                .ChildRules(i => i.RuleFor(r => r.RestrictionId).NotEmpty())
                .ChildRules(i => i.RuleFor(r => r.Value).NotEmpty())
                .When(i => i.IndexRestrictions != null);
        }
    }
}
