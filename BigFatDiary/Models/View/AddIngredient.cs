using BigFatDiary.Models.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigFatDiary.Models.View
{
    public class AddIngredient
    {
        [Required]
        [Display(Name = "Food")]
        public string FoodStuff { get; set; }

        [Required]
        [Range(0.0,Double.PositiveInfinity, ErrorMessage = "Amount must be non-negative")]
        public double Amount { get; set; }

        public ICollection<SelectListItem> FoodStuffs { get; set; }
        
    }
}