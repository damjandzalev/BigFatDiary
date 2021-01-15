using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BigFatDiary.Models.Data
{
    public class BigFatDiaryContext : DbContext
    {
        public DbSet<DiaryUser> DiaryUsers { get; set; }

        public DbSet<UserMeasurements> Measurements { get; set; }

        public DbSet<Review> Reviews { get; set; }

        public DbSet<FoodStuff> FoodStuffs { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<Ingredient> Ingredients { get; set; }

        public DbSet<FoodStuffEaten> FoodEatens { get; set; }

        public DbSet<RecipeEaten> RecipeEatens { get; set; }

        public DbSet<Bookmark> Bookmarks { get; set; }

        public BigFatDiaryContext() : base("DefaultConnection")
        {

        }
        public static BigFatDiaryContext Create()
        {
            return new BigFatDiaryContext();
        }

        public DiaryUser getDiaryUser(string username)
        {
            DiaryUser diaryUser = DiaryUsers.Find(username);
            if (diaryUser == null)
            {
                DateTime date = new DateTime();
                date = date.AddYears(1899);
                diaryUser = new DiaryUser()
                {
                    Username = username,
                    Birthday = date
                };
                DiaryUsers.Add(diaryUser);
                SaveChanges();
            }
            return diaryUser;
        }
    }
}