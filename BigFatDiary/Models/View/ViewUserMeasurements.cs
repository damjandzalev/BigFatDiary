using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigFatDiary.Models.View
{
    public class ViewUserMeasurements
    {
        [Required]
        [Display(Name = "Height(cm)")]
        public double Height { get; set; }

        [Required]
        [Display(Name = "Weight(kg)")]
        public double Weight { get; set; }

        [Required]
        [Range(1.0, 4.0, ErrorMessage = "Activity level must be in between 1.0 and 4.0")]
        [Display(Name = "Activity level")]
        public double ActivityLevel { get; set; }
    }
}