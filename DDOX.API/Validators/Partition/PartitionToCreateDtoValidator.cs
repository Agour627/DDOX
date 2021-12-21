using DDOX.API.Dtos.Partiton;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDOX.API.Validators.Partition
{
    public class PartitionToCreateDtoValidator : AbstractValidator<PartitionToCreateDto>
    {
        public PartitionToCreateDtoValidator()
        {
            RuleFor(p => p.Name).NotEmpty();
            RuleFor(p => p.Path).NotEmpty();
            RuleFor(p => p.CreatedBy).NotEmpty();
        }
    }
}
