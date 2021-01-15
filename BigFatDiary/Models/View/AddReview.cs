using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigFatDiary.Models.View
{
    public class AddReview
    {
        [Display(Name = "Grade")]
        [Range(1.0, 5.0, ErrorMessage = "Grade not within range![1.0-5.0]")]
        [Required]
        public double Grade { get; set; }

        [Display (Name = "Comment")]
        public string Comment { get; set; }

        [Display (Name = "Recipe")]
        public string RecipeName { get; set; }

        [Display (Name = "Recipe by")]
        public string RecipeCreator { get; set; }

        [Display]
        public long RecipeId { get; set; }
    }
}