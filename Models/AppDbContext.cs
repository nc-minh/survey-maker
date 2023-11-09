using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SurveyMaker.Models
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }

                builder.Entity<FormModel>()
                    .HasMany(f => f.Questions)
                    .WithOne(q => q.Form)
                    .HasForeignKey(q => q.FormId);

                builder.Entity<QuestionModel>()
                    .HasMany(q => q.Options)
                    .WithOne(o => o.Question)
                    .HasForeignKey(o => o.QuestionId);

                builder.Entity<ResponseModel>()
                    .HasOne(r => r.Form)
                    .WithMany(f => f.Responses)
                    .HasForeignKey(r => r.FormId);

                builder.Entity<AnswerModel>()
                    .HasOne(a => a.Option)
                    .WithMany(o => o.Answers)
                    .HasForeignKey(a => a.OptionId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<AnswerModel>()
                    .HasOne(a => a.Question)
                    .WithMany(q => q.Answers)
                    .HasForeignKey(a => a.QuestionId)
                    .OnDelete(DeleteBehavior.NoAction);

                builder.Entity<AnswerModel>()
                    .HasOne(a => a.Response)
                    .WithMany(r => r.Answers)
                    .HasForeignKey(a => a.ResponseId)
                    .OnDelete(DeleteBehavior.NoAction);

            }

        }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<FormModel> Forms { get; set; }

        public DbSet<QuestionModel> Questions { get; set; }

        public DbSet<OptionModel> Options { get; set; }

        public DbSet<ResponseModel> Responses { get; set; }

        public DbSet<AnswerModel> Answers { get; set; }

    }
}