using BigFatDiary.Models.Data;
using BigFatDiary.Models.Data.Enumerations;
using BigFatDiary.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace BigFatDiary.Controllers
{
    [Authorize(Roles = "Administrator,Moderator,User")]
    public class UserController : Controller
    {
        // GET: User
        private BigFatDiaryContext db = new BigFatDiaryContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddRecipe()
        {
            var Model = new AddRecipe();
            Model.Ingredients = new List<AddIngredient>();
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddRecipe(AddRecipe Model)
        {

            if (ModelState.IsValid)
            {
                DiaryUser diaryUser = db.getDiaryUser(User.Identity.Name);
                Recipe Recipe = new Recipe()
                {
                    Name = Model.Name,
                    Description = Model.Description,
                    CreatedBy = diaryUser,
                    Ingredients = new List<Ingredient>(),
                    Portions = Model.Portions
                };
                double calories = 0, carbs = 0, proteins = 0, fats = 0;
                List<AddIngredient> groupedIngredients = new List<AddIngredient>();
                //group same ingredients
                foreach (AddIngredient addIngredient in Model.Ingredients)
                {
                    int find = groupedIngredients.FindIndex(k => k.FoodStuff.Equals(addIngredient.FoodStuff));
                    if (find == -1)
                    {
                        groupedIngredients.Add(addIngredient);
                    }
                    else
                    {
                        AddIngredient ing = groupedIngredients.ElementAt(find);
                        ing.Amount += addIngredient.Amount;
                    }
                }
                //add ingredients to recipe object
                foreach (AddIngredient addIngredient in groupedIngredients)
                {
                    FoodStuff FoodStuff = db.FoodStuffs.Find(addIngredient.FoodStuff);
                    if (addIngredient.Amount > 0)
                    {
                        Recipe.Ingredients.Add(new Ingredient()
                        {
                            FoodStuff = FoodStuff,
                            Recipe = Recipe,
                            Amount = addIngredient.Amount
                        });
                    }
                    calories += FoodStuff.Calories * addIngredient.Amount;
                    carbs += FoodStuff.Carbohydrates * addIngredient.Amount;
                    proteins += FoodStuff.Proteins * addIngredient.Amount;
                    fats += FoodStuff.Fats * addIngredient.Amount;
                }
                Recipe.Calories = calories / Model.Portions;
                Recipe.Carbohydrates = carbs / Model.Portions;
                Recipe.Proteins = proteins / Model.Portions;
                Recipe.Fats = fats / Model.Portions;

                db.Recipes.Add(Recipe);
                db.Ingredients.AddRange(Recipe.Ingredients);
                db.SaveChanges();
                return RedirectToAction("DetailsRecipe/"+Recipe.Id);
            }
            List<SelectListItem> list = new List<SelectListItem>();
            foreach (FoodStuff fs in db.FoodStuffs)
            {
                list.Add(new SelectListItem()
                {
                    Text = fs.Name + ", " + fs.MeasurementUnit,
                    Value = fs.Name
                });
            }
            if (Model.Ingredients != null) { 
                foreach (AddIngredient AddIngredient in Model.Ingredients)
                {
                    AddIngredient.FoodStuffs = new List<SelectListItem>(list);
                }
            }
            else
            {
                Model.Ingredients = new List<AddIngredient>();
            }
            return View(Model);
        }

        public ActionResult AddIngredient()
        {
            var Model = new AddIngredient();
            Model.FoodStuffs = new List<SelectListItem>();
            foreach(FoodStuff fs in db.FoodStuffs)
            {
                Model.FoodStuffs.Add(new SelectListItem()
                {
                    Text = fs.Name+", "+fs.MeasurementUnit,
                    Value = fs.Name
                });
            }
            return PartialView("AddIngredient", Model);
        }

        public ActionResult ListRecipes()
        {
            List<DetailsRecipe> detailsRecipes = new List<DetailsRecipe>();
            List<Recipe> Recipes = new List<Recipe>(db.Recipes.Where(k => k.CreatedBy.Username.Equals(User.Identity.Name)));
            List<Bookmark> Bookmarks = new List<Bookmark>(db.Bookmarks.Where(k => k.DiaryUser.Username.Equals(User.Identity.Name)));
            foreach (Bookmark bookmark in Bookmarks)
            {
                Recipes.Add(bookmark.Recipe);
            }
            foreach(Recipe recipe in Recipes)
            {
                double grade = 0;
                int count = 0;
                foreach (Review review in db.Reviews.Where(k => k.Recipe.Id == recipe.Id))
                {
                    grade += review.Grade;
                    count++;
                }
                if(count > 0)
                    grade /= count;
                bool Bookmark = !recipe.CreatedBy.Username.Equals(User.Identity.Name);
                detailsRecipes.Add(new DetailsRecipe()
                {
                    Calories = recipe.Calories,
                    Carbohydrates = recipe.Carbohydrates,
                    Proteins = recipe.Proteins,
                    Fats = recipe.Fats,
                    Name = recipe.Name,
                    CreatedBy = recipe.CreatedBy.Username,
                    Portions = recipe.Portions,
                    Grade = grade,
                    Id = recipe.Id,
                    Bookmark = Bookmark
                });
            }
            ListRecipes Model = new ListRecipes()
            {
                IsSearch = false,
                SearchText = "",
                Recipes = detailsRecipes
            };
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ListRecipes(string searchText)
        {
            string text = HttpContext.Request.Params.Get("searchText");
            List<Recipe> recipes = db.Recipes.Where(k => k.Name.ToLower().StartsWith(text.ToLower())).ToList();
            recipes.AddRange(db.Recipes.Where(k => k.CreatedBy.Username.ToLower().StartsWith(text.ToLower())).ToList());
            ListRecipes Model = new ListRecipes();
            Model.IsSearch = true;
            Model.SearchText = text;
            Model.Recipes = new List<DetailsRecipe>();
            foreach (Recipe recipe in recipes)
            {
                double grade = 0;
                int count = 0;
                foreach (Review review in db.Reviews.Where(k => k.Recipe.Id == recipe.Id))
                {
                    grade += review.Grade;
                    count++;
                }
                if (count > 0)
                    grade /= count;
                bool Bookmark = !recipe.CreatedBy.Username.Equals(User.Identity.Name);
                Model.Recipes.Add(new DetailsRecipe()
                {
                    Calories = recipe.Calories,
                    Carbohydrates = recipe.Carbohydrates,
                    Proteins = recipe.Proteins,
                    Fats = recipe.Fats,
                    Name = recipe.Name,
                    CreatedBy = recipe.CreatedBy.Username,
                    Portions = recipe.Portions,
                    Grade = grade,
                    Id = recipe.Id,
                    Bookmark = Bookmark
                });
            }
            return View(Model);
        }

        public ActionResult DetailsRecipe(long id)
        {
            Recipe recipe = db.Recipes.Find(id);
            DetailsRecipe Model = null;
            if (recipe != null)
            {
                double grade = 0;
                int count = 0;
                foreach(Review review in db.Reviews.Where(k => k.Recipe.Id == recipe.Id))
                {
                    grade += review.Grade;
                    count++;
                }
                grade /= count;
                DiaryUser diaryUser = db.getDiaryUser(User.Identity.Name);
                bool bookmark = db.Bookmarks.ToList().Where(k => k.DiaryUser.Equals(diaryUser) && k.Recipe.Equals(recipe)).Any();
                Model = new DetailsRecipe()
                {
                    Id = recipe.Id,
                    Calories = recipe.Calories,
                    Carbohydrates = recipe.Carbohydrates,
                    Proteins = recipe.Proteins,
                    Fats = recipe.Fats,
                    Name = recipe.Name,
                    Description = recipe.Description,
                    CreatedBy = recipe.CreatedBy.Username,
                    Ingredients = new List<Ingredient>(recipe.Ingredients),
                    Reviews = new List<Review>(recipe.Reviews),
                    Portions = recipe.Portions,
                    Grade = grade,
                    Bookmark = bookmark,
                };
            }
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DetailsRecipe(AddReview Model)
        {
            Recipe recipe = db.Recipes.Find(Model.RecipeId);
            if (recipe == null)
            {
                return RedirectToAction("DetailsRecipe", Model.RecipeId);
            }
            DiaryUser diaryUser = db.getDiaryUser(User.Identity.Name);
            if (diaryUser.Username.Equals(recipe.CreatedBy))
            {
                return RedirectToAction("DetailsRecipe", Model.RecipeId);
            }
            if (ModelState.IsValid)
            {
                List<Review> reviews = db.Reviews.Where(k => k.Recipe.Id == Model.RecipeId && k.DiaryUser.Username.Equals(User.Identity.Name)).AsEnumerable().ToList();
                db.Reviews.RemoveRange(reviews);
                Review review = new Review()
                {
                    Recipe = recipe,
                    DiaryUser = diaryUser,
                    Comment = Model.Comment,
                    Grade = Model.Grade
                };
                db.Reviews.Add(review);
                db.SaveChanges();
                return Redirect("~/User/DetailsRecipe/" + Model.RecipeId);
            }
            DetailsRecipe detailsRecipe = null;
            if (recipe != null)
            {
                double grade = 0;
                int count = 0;
                foreach (Review review in db.Reviews.Where(k => k.Recipe.Id == recipe.Id))
                {
                    grade += review.Grade;
                    count++;
                }
                grade /= count;
                detailsRecipe = new DetailsRecipe()
                {
                    Id = recipe.Id,
                    Calories = recipe.Calories,
                    Carbohydrates = recipe.Carbohydrates,
                    Proteins = recipe.Proteins,
                    Fats = recipe.Fats,
                    Name = recipe.Name,
                    Description = recipe.Description,
                    CreatedBy = recipe.CreatedBy.Username,
                    Ingredients = new List<Ingredient>(recipe.Ingredients),
                    Reviews = new List<Review>(recipe.Reviews),
                    Portions = recipe.Portions,
                    Grade = grade
                };
            }
            detailsRecipe.AddReview = Model;
            return View(detailsRecipe);
        }

        //to do
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Eat(string id)
        {
            if (HttpContext.Request.Params.Get("type").Equals("Recipe"))
            {
                string amount = HttpContext.Request.Params.Get("amount");
                double k;
                if (double.TryParse(amount, out k))
                {
                    if (k > 0)
                    {
                        db.RecipeEatens.Add(new RecipeEaten()
                        {
                            Recipe = db.Recipes.Find(long.Parse(id)),
                            User = db.DiaryUsers.Find(User.Identity.Name),
                            Date = System.DateTime.Now,
                            Amount = k
                        });
                    }
                    db.SaveChanges();
                    return RedirectToAction("Diary", "User");
                }
                return RedirectToAction("DetailsRecipe/" + id.ToString());
            }
            else
            {
                string amount = HttpContext.Request.Params.Get("amount");
                double k;
                if (double.TryParse(amount, out k))
                {
                    if (k > 0)
                    {
                        db.FoodEatens.Add(new FoodStuffEaten()
                        {
                            FoodStuff = db.FoodStuffs.Find(id),
                            User = db.DiaryUsers.Find(User.Identity.Name),
                            Date = System.DateTime.Now,
                            Amount = k
                        });
                    }
                    db.SaveChanges();
                    return RedirectToAction("Diary", "User");
                }
                return RedirectToAction("Details/" + id.ToString(), "FoodStuffs");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bookmark(long id)
        {
            Recipe recipe = db.Recipes.Find(id);
            if(recipe != null)
            {
                DiaryUser diaryUser = db.getDiaryUser(User.Identity.Name);
                if (recipe.CreatedBy.Equals(diaryUser))
                {
                    return RedirectToAction("/DetailsRecipe/" + id.ToString());
                }
                Bookmark bookmark = null;
                if (db.Bookmarks.ToList().Where(k => k.DiaryUser.Equals(diaryUser) && k.Recipe.Equals(recipe)).Any())
                {
                    bookmark = db.Bookmarks.ToList().Where(k => k.DiaryUser.Equals(diaryUser) && k.Recipe.Equals(recipe)).First();
                }
                if(bookmark == null)
                {
                    db.Bookmarks.Add(new Bookmark()
                    {
                        DiaryUser = diaryUser,
                        Recipe = recipe
                    });
                }
                else
                {
                    db.Bookmarks.Remove(bookmark);
                }
                db.SaveChanges();
                return RedirectToAction("/DetailsRecipe/" + id.ToString());
            }
            return RedirectToAction("ListRecipes");
        }

        public ActionResult AddMeasurements()
        {
            var Model = new ViewUserMeasurements();
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMeasurements(ViewUserMeasurements Model)
        {
            if (ModelState.IsValid)
            {
                DiaryUser diaryUser = db.getDiaryUser(User.Identity.Name);
                DateTime dateTime = DateTime.Now.Date;
                List<UserMeasurements> userMeasurements = db.Measurements.Where(k => k.DateTime.Equals(dateTime) && k.DiaryUser.Username.Equals(User.Identity.Name)).ToList();
                if (userMeasurements == null)
                {
                    userMeasurements = new List<UserMeasurements>();
                }
                db.Measurements.RemoveRange(userMeasurements);
                db.Measurements.Add(new UserMeasurements()
                {
                    DiaryUser = diaryUser,
                    DateTime = dateTime,
                    Height = Model.Height,
                    Weight = Model.Weight,
                    ActivityLevel = Model.ActivityLevel
                });
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(Model);
        }

        public ActionResult Diary()
        {
            DiaryUser diaryUser = db.getDiaryUser(User.Identity.Name);
            List<RecipeEaten> recipeEatens = db.RecipeEatens.Where(k => k.User.Username.Equals(diaryUser.Username)).AsEnumerable().ToList();
            List<FoodStuffEaten> foodStuffEatens = db.FoodEatens.Where(k => k.User.Username.Equals(diaryUser.Username)).AsEnumerable().ToList();
            List<UserMeasurements> userMeasurements = db.Measurements.Where(k => k.DiaryUser.Username.Equals(diaryUser.Username)).AsEnumerable().ToList();
            recipeEatens.Sort((a, b) => a.Date.Date.CompareTo(b.Date.Date));
            foodStuffEatens.Sort((a, b) => a.Date.Date.CompareTo(b.Date.Date));
            userMeasurements.Sort((a, b) => a.DateTime.Date.CompareTo(b.DateTime.Date));
            DateTime date = System.DateTime.Now.Date;
            Diary Model = new Diary()
            {
                hasMeasurements = false
            };
            if (userMeasurements.Count() > 0)
            {
                List<DiaryDayDetails> diaryDayDetailsList = new List<DiaryDayDetails>();
                DateTime start = userMeasurements.First().DateTime.Date;
                double weight = userMeasurements.First().Weight;
                double height = userMeasurements.First().Height;
                double activityLevel = userMeasurements.First().ActivityLevel;
                while (start <= date)
                {
                    bool isWeightMeasured = false;
                    if (userMeasurements.Where(k => k.DateTime.Date.CompareTo(start) == 0).Any())
                    {
                        weight = userMeasurements.Where(k => k.DateTime.Date.CompareTo(start) == 0).First().Weight;
                        height = userMeasurements.Where(k => k.DateTime.Date.CompareTo(start) == 0).First().Height;
                        activityLevel = userMeasurements.Where(k => k.DateTime.Date.CompareTo(start) == 0).First().ActivityLevel;
                        isWeightMeasured = true;
                    }
                    double caloriesEaten = 0;
                    foreach (RecipeEaten k in recipeEatens.Where(k => k.Date.Date.CompareTo(start) == 0))
                    {
                        caloriesEaten += k.Amount * k.Recipe.Calories;
                    }
                    foreach (FoodStuffEaten k in foodStuffEatens.Where(k => k.Date.Date.CompareTo(start) == 0))
                    {
                        caloriesEaten += k.Amount * k.FoodStuff.Calories;
                    }
                    double caloriesToMaintain = 0;
                    if (diaryUser.Gender.Equals(Gender.MALE))
                    {
                        caloriesToMaintain = 10 * weight + 6.25 * height - 5 * (date.Year - diaryUser.Birthday.Year) + 5;
                    }
                    else
                    {
                        caloriesToMaintain = 10 * weight + 6.25 * height - 5 * (date.Year - diaryUser.Birthday.Year) - 161;
                    }
                    caloriesToMaintain *= activityLevel;
                    if (caloriesEaten == 0)
                        caloriesEaten = caloriesToMaintain;
                    double difference = caloriesToMaintain - caloriesEaten;
                    DiaryDayDetails diaryDayDetails = new DiaryDayDetails
                    {
                        caloriesEaten = caloriesEaten,
                        caloriesToMaintain = caloriesToMaintain,
                        difference = difference,
                        kilograms = weight,
                        DateTime = start,
                        weightMeasured = isWeightMeasured
                    };
                    diaryDayDetailsList.Add(diaryDayDetails);
                    weight -= difference / 7700;
                    start = start.Date.AddDays(1);
                }
                Model.hasMeasurements = true;
                Model.DayDetails = diaryDayDetailsList;
            }
            return View(Model);
        }

        public ActionResult DiaryUser()
        {
            var Model = db.getDiaryUser(User.Identity.Name);
            return View(Model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DiaryUser(DiaryUser Model)
        {
            if (ModelState.IsValid)
            {
                db.DiaryUsers.Find(User.Identity.Name).Birthday = Model.Birthday;
                db.DiaryUsers.Find(User.Identity.Name).Gender = Model.Gender;
                db.SaveChanges();
                return RedirectToAction("Diary", "User");
            }
            return View(Model);
        }
    }
}