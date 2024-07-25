using Microsoft.EntityFrameworkCore;

namespace Questionare.models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Setting table names and schema
            modelBuilder.Entity<Question>().ToTable("tblQuestions", "dbo");
            modelBuilder.Entity<Option>().ToTable("tblOption", "dbo");
            modelBuilder.Entity<Answer>().ToTable("tblCorrectAnswer", "dbo");

            // Setting default values and ensuring that no null values are allowed for specific columns
            modelBuilder.Entity<Question>()
                .Property(q => q.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired(); // Ensuring this column cannot be null

            modelBuilder.Entity<Option>()
                .Property(o => o.IsDeleted)
                .HasDefaultValue(false)
                .IsRequired(); // Ensuring this column cannot be null

            modelBuilder.Entity<Option>()
                .Property(o => o.IsCorrect)
                .HasDefaultValue(false)
                .IsRequired(); // Ensuring this column cannot be null

            // If you have any relationships or keys to set up, do that here as well
            // For example, if 'Option' should have a foreign key back to 'Question':
            modelBuilder.Entity<Option>()
                .HasOne<Question>() // Assuming Option has a navigation property to Question
                .WithMany(q => q.Options) // Assuming Question has a navigation collection of Options
                .HasForeignKey(o => o.QuestionId) // Assuming Option has a foreign key property named 'QuestionId'
                .OnDelete(DeleteBehavior.Cascade); // Optional: specify the delete behavior

            // For the Answer table, if it relates to both questions and options:
            modelBuilder.Entity<Answer>()
                .HasKey(a => new { a.QuestionId, a.OptionId }); // Configuring a composite key, assuming Answer uses both IDs

            // You can uncomment this section if needed:
            // modelBuilder.Entity<CorrectOption>()
            //    .HasKey(c => new { c.QuestionId, c.OptionId });
        }
    }
}
