namespace MOAS_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedactivitytypesandchangedactivitytypepropinactivitymodel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        AllowUploads = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ActivityModels", "ActivityType_Id", c => c.Int());
            CreateIndex("dbo.ActivityModels", "ActivityType_Id");
            AddForeignKey("dbo.ActivityModels", "ActivityType_Id", "dbo.ActivityTypes", "Id");
            DropColumn("dbo.ActivityModels", "ActivityType");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ActivityModels", "ActivityType", c => c.String());
            DropForeignKey("dbo.ActivityModels", "ActivityType_Id", "dbo.ActivityTypes");
            DropIndex("dbo.ActivityModels", new[] { "ActivityType_Id" });
            DropColumn("dbo.ActivityModels", "ActivityType_Id");
            DropTable("dbo.ActivityTypes");
        }
    }
}
