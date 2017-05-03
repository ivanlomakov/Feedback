namespace Conference.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsRequired : DbMigration
    {
        public override void Up()
        {
            this.AddColumn("dbo.Questions", "IsRequired", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            this.DropColumn("dbo.Questions", "IsRequired");
        }
    }
}
