using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigFatDiary.Models.View
{
    public class AddRecipe
    {
        [Required]
        [Display(Name = "Recipe name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Recipe")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Ingredients")]
        public ICollection<AddIngredient> Ingredients { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        [Display(Name ="Portions")]
        public int Portions { get; set; }
    }
}