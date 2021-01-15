using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BigFatDiary.Models.Data
{
    public class Ingredient
    {

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        public virtual Recipe Recipe { get; set; }

        [Required]
        public virtual FoodStuff FoodStuff { get; set; }

        [Required]
        [Range(0.0, double.PositiveInfinity, ErrorMessage = "Amount must be non-negative.")]
        public double Amount { get; set; }
    }
}