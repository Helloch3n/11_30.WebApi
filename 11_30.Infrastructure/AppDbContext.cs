using _11_30.Domain.Entities.GeneralQuery;
using _11_30.Domain.Entities.QuestionBank;
using Microsoft.EntityFrameworkCore;
using System;

namespace _11_30.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public DbSet<QuestionBank> QuestionBanks => Set<QuestionBank>();
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<GeneralQueryAction> GeneralQueryActions => Set<GeneralQueryAction>();
        public DbSet<GeneralQueryField> GeneralQueryFields => Set<GeneralQueryField>();
        public DbSet<GeneralQueryType> GeneralQueryTypes => Set<GeneralQueryType>();
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //加载当前程序集的实体类
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
