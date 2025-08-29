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
    public class GeneralQueryTypeConfiguration : IEntityTypeConfiguration<GeneralQueryType>
    {
        public void Configure(EntityTypeBuilder<GeneralQueryType> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(50);
            builder.Property(x => x.Url).IsRequired(false).HasMaxLength(200);
            builder.Property(x => x.RequestContent).IsRequired(false);
            builder.HasMany(x => x.Actions).WithOne().HasForeignKey(a => a.GeneralQueryTypeId).OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("GeneralQueryType");
        }
    }
}
