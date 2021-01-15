using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BigFatDiary.Models.Data;

namespace BigFatDiary.Controllers
{
    [Authorize(Roles = "Administrator,Moderator,User")]
    public class FoodStuffsController : Controller
    {
        private BigFatDiaryContext db = new BigFatDiaryContext();

        // GET: FoodStuffs
        public ActionResult Index()
        {
            return View(db.FoodStuffs.ToList());
        }

        // GET: FoodStuffs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodStuff foodStuff = db.FoodStuffs.Find(id);
            if (foodStuff == null)
            {
                return HttpNotFound();
            }
            return View(foodStuff);
        }

        [Authorize(Roles = "Administrator,Moderator")]
        // GET: FoodStuffs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FoodStuffs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Create([Bind(Include = "Name,Calories,Proteins,Carbohydrates,Fats,MeasurementUnit")] FoodStuff foodStuff)
        {
            if (ModelState.IsValid)
            {
                foodStuff.AddedBy = User.Identity.Name;
                db.FoodStuffs.Add(foodStuff);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(foodStuff);
        }

        // GET: FoodStuffs/Edit/5
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FoodStuff foodStuff = db.FoodStuffs.Find(id);
            if (foodStuff == null)
            {
                return HttpNotFound();
            }
            return View(foodStuff);
        }

        // POST: FoodStuffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator,Moderator")]
        public ActionResult Edit([Bind(Include = "Name,Calories,Proteins,Carbohydrates,Fats,MeasurementUnit")] FoodStuff foodStuff)
        {
            if (ModelState.IsValid)
            {
                foodStuff.AddedBy = User.Identity.Name;
                db.Entry(foodStuff).State = EntityState.Modified;
                List<Recipe> recipes = new List<Recipe>(db.Recipes);
                foreach (Recipe recipe in recipes.Where(k => k.Ingredients.Where(p => p.FoodStuff.Name.Equals(foodStuff.Name)).Any())){
                    double calories = 0, carbs = 0, proteins = 0, fats = 0;
                    foreach(Ingredient ingredient in recipe.Ingredients)
                    {
                        calories += ingredient.FoodStuff.Calories * ingredient.Amount;
                        carbs += ingredient.FoodStuff.Carbohydrates * ingredient.Amount;
                        proteins += ingredient.FoodStuff.Proteins * ingredient.Amount;
                        fats += ingredient.FoodStuff.Fats * ingredient.Amount;
                    }
                    recipe.Calories = calories / recipe.Portions;
                    recipe.Carbohydrates = carbs / recipe.Portions;
                    recipe.Proteins = proteins / recipe.Portions;
                    recipe.Fats = fats / recipe.Portions;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(foodStuff);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
