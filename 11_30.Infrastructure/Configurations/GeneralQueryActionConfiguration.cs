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
    public class GeneralQueryActionConfiguration : IEntityTypeConfiguration<GeneralQueryAction>
    {
        public void Configure(EntityTypeBuilder<GeneralQueryAction> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(100);
            builder.HasMany(x => x.Fields).WithOne().HasForeignKey(o => o.GeneralQueryActionId).OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("GeneralQueryAction");
        }
    }
}
