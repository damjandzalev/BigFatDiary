using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BigFatDiary.Models.Data
{
    public class Recipe
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Created by")]
        public virtual DiaryUser CreatedBy { get; set; }

        [Required]
        [Display(Name = "Recipe name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Recipe description")]
        public string Description { get; set; }

        [Display(Name = "Calories(per portion)")]
        public double Calories { get; set; }

        [Range(0.0, 100.0)]
        [Display(Name = "Protein(grams per portion)")]
        public double Proteins { get; set; }

        [Display(Name = "Carbohydrates(grams per portion)")]
        [Range(0.0, double.MaxValue)]
        public double Carbohydrates { get; set; }

        [Range(0.0, double.MaxValue)]
        [Display(Name = "Fats(grams per portion)")]
        public double Fats { get; set; }

        [Required]
        [Display(Name="Portions")]
        [Range(1, int.MaxValue)]
        public int Portions { get; set; }

        public virtual ICollection<Ingredient> Ingredients { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}