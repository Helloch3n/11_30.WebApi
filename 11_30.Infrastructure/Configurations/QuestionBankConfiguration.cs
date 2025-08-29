using _11_30.Domain.Entities.QuestionBank;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _11_30.Infrastructure.Configurations
{
    internal class QuestionBankConfiguration : IEntityTypeConfiguration<QuestionBank>
    {
        public void Configure(EntityTypeBuilder<QuestionBank> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.XPathEndStr).IsRequired().HasMaxLength(300);
            builder.HasMany(x => x.Questions).WithOne().HasForeignKey(a => a.QuestionBankId).OnDelete(DeleteBehavior.Cascade);
            builder.Property(x => x.IsOnGoing);

            builder.ToTable("QuestionBank");
        }
    }
}
