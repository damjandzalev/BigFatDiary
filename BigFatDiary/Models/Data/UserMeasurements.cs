using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigFatDiary.Models.Data
{
    public class UserMeasurements
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public virtual DiaryUser DiaryUser { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        [Display(Name = "Height")]
        public double Height { get; set; }

        [Required]
        [Display(Name = "Weight")]
        public double Weight { get; set; }

        [Required]
        [Range(1.0, 4.0, ErrorMessage = "Activity level must be in between 1.0 and 4.0")]
        [Display(Name = "Activity level")]
        public double ActivityLevel { get; set; }

    }
}