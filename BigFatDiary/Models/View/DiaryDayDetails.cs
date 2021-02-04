using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigFatDiary.Models.View
{
    public class DiaryDayDetails
    {
        [Display(Name="Date")]
        public DateTime DateTime { get; set; }

        [Display(Name = "Calories To Maintain")]
        public double caloriesToMaintain { get; set; }

        [Display(Name ="Calories Eaten")]
        public double caloriesEaten { get; set; }

        [Display(Name ="Difference")]
        public double difference { get; set; }

        [Display(Name ="Approximate kg")]
        public double kilograms { get; set; }

        [Display(Name ="Is weight measured")]
        public bool weightMeasured { get; set; }
    }
}