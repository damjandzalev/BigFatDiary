using BigFatDiary.Models.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigFatDiary.Models.View
{
    public class AddFood
    {
        [Display(Name = "Food")]
        public string Name { get; set; }

        [Required]
        [Range(0.0, double.MaxValue)]
        [Display(Name = "Calories(per measurement unit)")]
        public double Calories { get; set; }

        [Required]
        [Range(0.0, 100.0)]
        [Display(Name = "Protein(per measurement unit)")]
        public double Proteins { get; set; }

        [Required]
        [Range(0.0, 100.0)]
        [Display(Name = "Carbohydrates(per measurement unit)")]
        public double Carbohydrates { get; set; }

        [Required]
        [Range(0.0, 100.0)]
        [Display(Name = "Fats(per measurement unit)")]
        public double Fats { get; set; }


        [Required]
        [Display(Name = "Measurement unit")]
        public MeasurementUnit MeasurementUnit;
    }
}