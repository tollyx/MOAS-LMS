namespace MOAS_LMS.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddActivities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ActivityModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ActivityType = c.String(),
                        Name = c.String(),
                        StartDate = c.DateTime(nullable: false),
                        EndDate = c.DateTime(nullable: false),
                        Description = c.String(),
                        Module_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ModuleModels", t => t.Module_Id)
                .Index(t => t.Module_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ActivityModels", "Module_Id", "dbo.ModuleModels");
            DropIndex("dbo.ActivityModels", new[] { "Module_Id" });
            DropTable("dbo.ActivityModels");
        }
    }
}
