using DDOX.API.Infrastructure.Enums;
using DDOX.API.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Repository.Configuration
{
    public class RestrictionConfiguration : IEntityTypeConfiguration<Restriction>
    {
        public void Configure(EntityTypeBuilder<Restriction> builder)
        {
            var restrictions = new List<Restriction>();
            foreach (int item in Enum.GetValues(typeof(RestrictionType)))
            {
                var _restriction = new Restriction
                {
                    Name = Enum.GetName(typeof(RestrictionType), item),
                    Id = item
                };
                restrictions.Add(_restriction);
            }
            builder.HasData(restrictions);
        }
    }

}
