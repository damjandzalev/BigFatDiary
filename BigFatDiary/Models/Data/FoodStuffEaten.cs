using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BigFatDiary.Models.Data
{
    public class FoodStuffEaten
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Food eaten")]
        public virtual FoodStuff FoodStuff { get; set; }

        [Required]
        public virtual DiaryUser User { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [Range(0.0,double.MaxValue,ErrorMessage = "Enter number greater or equal to 0.")]
        [Display(Name = "Amount")]
        public double Amount { get; set; }
    }
}