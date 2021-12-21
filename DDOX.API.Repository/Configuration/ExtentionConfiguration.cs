using DDOX.API.Infrastructure.Configurations;
using DDOX.API.Infrastructure.Models;
using DDOX.API.Repository.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDOX.API.Repository.Configuration
{
    public class ExtentionConfiguration : IEntityTypeConfiguration<Extension>
    {
        public void Configure(EntityTypeBuilder<Extension> builder)
        {
            builder.HasData(
                             new Extension { Id = 1, Name = ".csv", MimeType = "text/csv" ,IsAllowed=true},
                             new Extension { Id = 2, Name = ".doc", MimeType = "application/msword" , IsAllowed =true},
                             new Extension { Id = 3, Name = ".gif", MimeType = "image/gif" ,IsAllowed=true},
                             new Extension { Id = 4, Name = ".docx", MimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document", IsAllowed=true },
                             new Extension { Id = 5, Name = ".ico", MimeType = "image/vnd.microsoft.icon" ,IsAllowed=true},
                             new Extension { Id = 6, Name = ".jpeg", MimeType = "image/jpeg", IsAllowed = true },
                             new Extension { Id = 7, Name = ".jpg", MimeType = "image/jpeg", IsAllowed = true },
                             new Extension { Id = 8, Name = ".jfif", MimeType = "image/jfif", IsAllowed = true },
                             new Extension { Id = 9, Name = ".png", MimeType = "image/png", IsAllowed = true },
                             new Extension { Id = 10, Name = ".pdf", MimeType = "application/pdf", IsAllowed = true },
                             new Extension { Id = 11, Name = ".rar", MimeType = "application/vnd.rar", IsAllowed = true },
                             new Extension { Id = 12, Name = ".txt", MimeType = "text/plain", IsAllowed = true },
                             new Extension { Id = 13, Name = ".xls", MimeType = "application/vnd.ms-excel" , IsAllowed = true },
                             new Extension { Id = 14, Name = ".xlsx", MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", IsAllowed = true }
                            );

        }
    }
}
