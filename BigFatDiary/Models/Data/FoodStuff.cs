using BigFatDiary.Models.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigFatDiary.Models.Data
{
    public class FoodStuff
    {
        [Key]
        [Display(Name = "Food name")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Added by")]
        public string AddedBy { get; set; }

        [Required]
        [Range(0.0, double.MaxValue)]
        [Display(Name = "Calories")]
        public double Calories { get; set; }

        [Required]
        [Range(0.0, 100.0)]
        [Display(Name = "Protein")]
        public double Proteins { get; set; }

        [Required]
        [Range(0.0, 100.0)]
        [Display(Name = "Carbohydrates")]
        public double Carbohydrates { get; set; }

        [Required]
        [Range(0.0, 100.0)]
        [Display(Name = "Fats")]
        public double Fats { get; set; }

        [Required]
        [Display(Name = "Measurement unit")]
        public MeasurementUnit MeasurementUnit { get; set; }
    }
}