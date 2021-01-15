using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigFatDiary.Models.Data
{
    public class RecipeEaten
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Recipe eaten")]
        public virtual Recipe Recipe { get; set; }

        [Required]
        public virtual DiaryUser User { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Enter number greater or equal to 0.")]
        [Display(Name = "Amount(Number of portions - decimal values allowed)")]
        public double Amount { get; set; }
    }
}