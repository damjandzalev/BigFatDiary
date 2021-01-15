namespace BigFatDiary.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FoodStuffEatens",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Double(nullable: false),
                        FoodStuff_Name = c.String(nullable: false, maxLength: 128),
                        User_Username = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FoodStuffs", t => t.FoodStuff_Name, cascadeDelete: true)
                .ForeignKey("dbo.DiaryUsers", t => t.User_Username, cascadeDelete: true)
                .Index(t => t.FoodStuff_Name)
                .Index(t => t.User_Username);
            
            CreateTable(
                "dbo.FoodStuffs",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        AddedBy = c.String(),
                        Calories = c.Double(nullable: false),
                        Proteins = c.Double(nullable: false),
                        Carbohydrates = c.Double(nullable: false),
                        Fats = c.Double(nullable: false),
                        MeasurementUnit = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.DiaryUsers",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        Birthday = c.DateTime(nullable: false),
                        Gender = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Username);
            
            CreateTable(
                "dbo.Bookmarks",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DiaryUser_Username = c.String(maxLength: 128),
                        Recipe_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiaryUsers", t => t.DiaryUser_Username)
                .ForeignKey("dbo.Recipes", t => t.Recipe_Id)
                .Index(t => t.DiaryUser_Username)
                .Index(t => t.Recipe_Id);
            
            CreateTable(
                "dbo.Recipes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Description = c.String(nullable: false),
                        Calories = c.Double(nullable: false),
                        Proteins = c.Double(nullable: false),
                        Carbohydrates = c.Double(nullable: false),
                        Fats = c.Double(nullable: false),
                        Portions = c.Int(nullable: false),
                        CreatedBy_Username = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiaryUsers", t => t.CreatedBy_Username, cascadeDelete: false)
                .Index(t => t.CreatedBy_Username);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Double(nullable: false),
                        FoodStuff_Name = c.String(nullable: false, maxLength: 128),
                        Recipe_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FoodStuffs", t => t.FoodStuff_Name, cascadeDelete: true)
                .ForeignKey("dbo.Recipes", t => t.Recipe_Id, cascadeDelete: true)
                .Index(t => t.FoodStuff_Name)
                .Index(t => t.Recipe_Id);
            
            CreateTable(
                "dbo.RecipeEatens",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Amount = c.Double(nullable: false),
                        Recipe_Id = c.Long(nullable: false),
                        User_Username = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Recipes", t => t.Recipe_Id, cascadeDelete: true)
                .ForeignKey("dbo.DiaryUsers", t => t.User_Username, cascadeDelete: true)
                .Index(t => t.Recipe_Id)
                .Index(t => t.User_Username);
            
            CreateTable(
                "dbo.UserMeasurements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Height = c.Double(nullable: false),
                        Weight = c.Double(nullable: false),
                        ActivityLevel = c.Double(nullable: false),
                        DiaryUser_Username = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiaryUsers", t => t.DiaryUser_Username, cascadeDelete: true)
                .Index(t => t.DiaryUser_Username);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Grade = c.Double(nullable: false),
                        Comment = c.String(),
                        DiaryUser_Username = c.String(nullable: false, maxLength: 128),
                        Recipe_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DiaryUsers", t => t.DiaryUser_Username, cascadeDelete: true)
                .ForeignKey("dbo.Recipes", t => t.Recipe_Id, cascadeDelete: true)
                .Index(t => t.DiaryUser_Username)
                .Index(t => t.Recipe_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "Recipe_Id", "dbo.Recipes");
            DropForeignKey("dbo.Reviews", "DiaryUser_Username", "dbo.DiaryUsers");
            DropForeignKey("dbo.FoodStuffEatens", "User_Username", "dbo.DiaryUsers");
            DropForeignKey("dbo.UserMeasurements", "DiaryUser_Username", "dbo.DiaryUsers");
            DropForeignKey("dbo.RecipeEatens", "User_Username", "dbo.DiaryUsers");
            DropForeignKey("dbo.RecipeEatens", "Recipe_Id", "dbo.Recipes");
            DropForeignKey("dbo.Bookmarks", "Recipe_Id", "dbo.Recipes");
            DropForeignKey("dbo.Ingredients", "Recipe_Id", "dbo.Recipes");
            DropForeignKey("dbo.Ingredients", "FoodStuff_Name", "dbo.FoodStuffs");
            DropForeignKey("dbo.Recipes", "CreatedBy_Username", "dbo.DiaryUsers");
            DropForeignKey("dbo.Bookmarks", "DiaryUser_Username", "dbo.DiaryUsers");
            DropForeignKey("dbo.FoodStuffEatens", "FoodStuff_Name", "dbo.FoodStuffs");
            DropIndex("dbo.Reviews", new[] { "Recipe_Id" });
            DropIndex("dbo.Reviews", new[] { "DiaryUser_Username" });
            DropIndex("dbo.UserMeasurements", new[] { "DiaryUser_Username" });
            DropIndex("dbo.RecipeEatens", new[] { "User_Username" });
            DropIndex("dbo.RecipeEatens", new[] { "Recipe_Id" });
            DropIndex("dbo.Ingredients", new[] { "Recipe_Id" });
            DropIndex("dbo.Ingredients", new[] { "FoodStuff_Name" });
            DropIndex("dbo.Recipes", new[] { "CreatedBy_Username" });
            DropIndex("dbo.Bookmarks", new[] { "Recipe_Id" });
            DropIndex("dbo.Bookmarks", new[] { "DiaryUser_Username" });
            DropIndex("dbo.FoodStuffEatens", new[] { "User_Username" });
            DropIndex("dbo.FoodStuffEatens", new[] { "FoodStuff_Name" });
            DropTable("dbo.Reviews");
            DropTable("dbo.UserMeasurements");
            DropTable("dbo.RecipeEatens");
            DropTable("dbo.Ingredients");
            DropTable("dbo.Recipes");
            DropTable("dbo.Bookmarks");
            DropTable("dbo.DiaryUsers");
            DropTable("dbo.FoodStuffs");
            DropTable("dbo.FoodStuffEatens");
        }
    }
}
