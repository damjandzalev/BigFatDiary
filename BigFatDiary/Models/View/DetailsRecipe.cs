using BigFatDiary.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigFatDiary.Models.View
{
    public class DetailsRecipe
    {
        public long Id { get; set; }

        [Display(Name = "Created by")]
        public string CreatedBy { get; set; }

        [Display(Name = "Recipe name")]
        public string Name { get; set; }

        [Display(Name = "Recipe description")]
        public string Description { get; set; }

        [Display(Name = "Calories(portion)")]
        public double Calories { get; set; }

        [Display(Name = "Protein(g)")]
        public double Proteins { get; set; }

        [Display(Name = "Carbohydrates(g)")]
        public double Carbohydrates { get; set; }

        [Display(Name = "Fats(g)")]
        public double Fats { get; set; }

        [Display(Name = "Portions")]
        public int Portions { get; set; }

        [Display(Name = "Grade")]
        public double Grade { get; set; }

        [Display(Name = "Bookmark")]
        public bool Bookmark { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }

        public AddReview AddReview { get; set; }
    }
}