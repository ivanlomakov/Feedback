using System.Data.Entity;
using Conference.Core.Entities;
using Conference.Core.Entities.Answers;
using Conference.Core.Entities.Questions;
using Conference.Core.Entities.Questions.Options;

namespace Conference.DAL
{
    public class ConferenceContext : DbContext
    {
        public ConferenceContext() : base("Conference")
        {
        }

        public DbSet<Answer> Answers { get; set; }

        public DbSet<TextAnswer> TextAnswers { get; set; }

        public DbSet<SingleOptionAnswer> SingleOptionAnswers { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<QuestionOption> QuestionOptions { get; set; }

        public DbSet<SingleOptionQuestion> SingleOptionQuestion { get; set; }

        public DbSet<RatingOptionQuestion> RatingOptionQuestions { get; set; }

        public DbSet<TextQuestion> TextQuestion { get; set; }

        public DbSet<Respondent> Respondents { get; set; }

        public DbSet<SelectedUid> SelectedUids { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(new ForeignKeyNamingConvention());

            base.OnModelCreating(modelBuilder);
        }
    }
}