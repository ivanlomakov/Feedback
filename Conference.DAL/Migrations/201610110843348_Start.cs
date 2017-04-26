using System.Data.Entity.Migrations;

namespace Conference.DAL.Migrations
{
    public partial class Start : DbMigration
    {
        public override void Up()
        {
            this.CreateTable(
                "dbo.Answers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Uid = c.String(),
                        OptionText = c.String(),
                        Text = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        QuestionId = c.Int(),
                        RespondentId = c.Int(),
                        OptionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId)
                .ForeignKey("dbo.Respondents", t => t.RespondentId)
                .ForeignKey("dbo.QuestionOptions", t => t.OptionId)
                .Index(t => t.QuestionId, name: "IX_Question_Id")
                .Index(t => t.RespondentId, name: "IX_Respondent_Id")
                .Index(t => t.OptionId, name: "IX_Option_Id");
            
            this.CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Order = c.Int(),
                        Description = c.String(),
                        From = c.Int(),
                        To = c.Int(),
                        Step = c.Int(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        RatingQuestionId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.RatingQuestionId)
                .Index(t => t.RatingQuestionId, name: "IX_RatingQuestion_Id");

            this.CreateTables();
        }

        private void CreateTables()
        {
            this.CreateTable(
                "dbo.QuestionOptions",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Text = c.String(),
                    InputText = c.String(),
                    Discriminator = c.String(nullable: false, maxLength: 128),
                    RedirectId = c.Int(),
                    SingleOptionQuestionId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.RedirectId)
                .ForeignKey("dbo.Questions", t => t.SingleOptionQuestionId)
                .Index(t => t.RedirectId, name: "IX_Redirect_Id")
                .Index(t => t.SingleOptionQuestionId, name: "IX_SingleOptionQuestion_Id");

            this.CreateTable(
                "dbo.Respondents",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Uid = c.String(),
                    SessionId = c.Guid(nullable: false),
                    CurrentQuestionId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.CurrentQuestionId)
                .Index(t => t.CurrentQuestionId, name: "IX_CurrentQuestion_Id");

            this.CreateTable(
                "dbo.RatingOptionAnswers",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Rating = c.Int(nullable: false),
                    RatingOptionQuestionId = c.Int(),
                    RatingAnswerId = c.Int(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.RatingOptionQuestionId)
                .ForeignKey("dbo.Answers", t => t.RatingAnswerId)
                .Index(t => t.RatingOptionQuestionId, name: "IX_RatingOptionQuestion_Id")
                .Index(t => t.RatingAnswerId, name: "IX_RatingAnswer_Id");

            this.CreateAdditionalTables();
        }

        private void CreateAdditionalTables()
        {
            this.CreateTable(
                "dbo.SelectedUids",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Uid = c.String(),
                })
                .PrimaryKey(t => t.Id);
        }

        public override void Down()
        {
            this.DropForeignKey("dbo.Answers", "OptionId", "dbo.QuestionOptions");
            this.DropForeignKey("dbo.RatingOptionAnswers", "RatingAnswerId", "dbo.Answers");
            this.DropForeignKey("dbo.RatingOptionAnswers", "RatingOptionQuestionId", "dbo.Questions");
            this.DropForeignKey("dbo.Answers", "RespondentId", "dbo.Respondents");
            this.DropForeignKey("dbo.Respondents", "CurrentQuestionId", "dbo.Questions");
            this.DropForeignKey("dbo.Answers", "QuestionId", "dbo.Questions");
            this.DropForeignKey("dbo.QuestionOptions", "SingleOptionQuestionId", "dbo.Questions");
            this.DropForeignKey("dbo.QuestionOptions", "RedirectId", "dbo.Questions");
            this.DropForeignKey("dbo.Questions", "RatingQuestionId", "dbo.Questions");

            this.DropIndex("dbo.RatingOptionAnswers", "IX_RatingAnswer_Id");
            this.DropIndex("dbo.RatingOptionAnswers", "IX_RatingOptionQuestion_Id");
            this.DropIndex("dbo.Respondents", "IX_CurrentQuestion_Id");
            this.DropIndex("dbo.QuestionOptions", "IX_SingleOptionQuestion_Id");
            this.DropIndex("dbo.QuestionOptions", "IX_Redirect_Id");
            this.DropIndex("dbo.Questions", "IX_RatingQuestion_Id");
            this.DropIndex("dbo.Answers", "IX_Option_Id");
            this.DropIndex("dbo.Answers", "IX_Respondent_Id");
            this.DropIndex("dbo.Answers", "IX_Question_Id");

            this.DropTable("dbo.SelectedUids");
            this.DropTable("dbo.RatingOptionAnswers");
            this.DropTable("dbo.Respondents");
            this.DropTable("dbo.QuestionOptions");
            this.DropTable("dbo.Questions");
            this.DropTable("dbo.Answers");
        }
    }
}
