using DDOX.API.Dtos.Partition;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Validators.Partition
{
    public class PartitionToUpdateDtoValidator : AbstractValidator<PartitionToUpdateDto>
    {
        public PartitionToUpdateDtoValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.UpdatedBy).NotEmpty();
        }
    }
}
