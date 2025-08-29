using _11_30.Domain.Entities.GeneralQuery;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Infrastructure.Configurations
{
    public class GeneralQueryFieldConfiguration : IEntityTypeConfiguration<GeneralQueryField>
    {
        public void Configure(EntityTypeBuilder<GeneralQueryField> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name);
            builder.Property(x => x.Description);

            builder.ToTable("GeneralQueryField");
        }
    }
}
