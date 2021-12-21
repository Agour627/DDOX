using DDOX.API.Dtos.Index;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Validators.Index
{
    public class IndexToUpdateDtoValidator : AbstractValidator<IndexToUpdateDto>
    {
        public IndexToUpdateDtoValidator()
        {
            RuleFor(i => i.Label).NotEmpty();
            RuleFor(i => i.UpdatedBy).NotEmpty();
            RuleForEach(i => i.IndexRestrictions)
                .ChildRules(i => i.RuleFor(r => r.RestrictionId).NotEmpty())
                .ChildRules(i => i.RuleFor(r => r.Value).NotEmpty())
                .When(i => i.IndexRestrictions != null);
        }
    }
}
