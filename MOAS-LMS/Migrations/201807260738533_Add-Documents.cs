namespace MOAS_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDocuments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DocumentModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        Path = c.String(),
                        TimeStamp = c.DateTime(nullable: false),
                        Feedback = c.String(),
                        IsHandIn = c.Boolean(nullable: false),
                        Activity_Id = c.Int(),
                        Course_Id = c.Int(),
                        Module_Id = c.Int(),
                        Uploader_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ActivityModels", t => t.Activity_Id)
                .ForeignKey("dbo.CourseModels", t => t.Course_Id)
                .ForeignKey("dbo.ModuleModels", t => t.Module_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Uploader_Id)
                .Index(t => t.Activity_Id)
                .Index(t => t.Course_Id)
                .Index(t => t.Module_Id)
                .Index(t => t.Uploader_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DocumentModels", "Uploader_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.DocumentModels", "Module_Id", "dbo.ModuleModels");
            DropForeignKey("dbo.DocumentModels", "Course_Id", "dbo.CourseModels");
            DropForeignKey("dbo.DocumentModels", "Activity_Id", "dbo.ActivityModels");
            DropIndex("dbo.DocumentModels", new[] { "Uploader_Id" });
            DropIndex("dbo.DocumentModels", new[] { "Module_Id" });
            DropIndex("dbo.DocumentModels", new[] { "Course_Id" });
            DropIndex("dbo.DocumentModels", new[] { "Activity_Id" });
            DropTable("dbo.DocumentModels");
        }
    }
}
