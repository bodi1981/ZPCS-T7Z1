namespace Diary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActivitiesToStudentTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Activities", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "Activities");
        }
    }
}
