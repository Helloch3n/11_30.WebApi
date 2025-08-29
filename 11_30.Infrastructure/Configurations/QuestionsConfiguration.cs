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
    public class QuestionsConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Title).IsRequired().HasMaxLength(300);
            builder.Property(x => x.Answers).IsRequired().HasMaxLength(20);
            builder.Property(x => x.QuestionType);
            builder.Property(x => x.OptionA).IsRequired().HasMaxLength(300);
            builder.Property(x => x.OptionB).IsRequired().HasMaxLength(300);
            builder.Property(x => x.OptionC).IsRequired().HasMaxLength(300);
            builder.Property(x => x.OptionD).IsRequired().HasMaxLength(300);
            builder.Property(x => x.OptionE).IsRequired().HasMaxLength(300);
            builder.Property(x => x.OptionF).IsRequired().HasMaxLength(300);

            builder.ToTable("Questions");
        }
    }
}
