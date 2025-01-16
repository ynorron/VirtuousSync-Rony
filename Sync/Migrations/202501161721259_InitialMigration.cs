namespace Sync.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AbbreviatedContacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExternalId = c.Int(nullable: false),
                        Name = c.String(),
                        ContactType = c.String(),
                        ContactName = c.String(),
                        Address = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.AbbreviatedContacts");
        }
    }
}
