using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BigFatDiary.Models.Data
{
    public class Review
    {

        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public virtual Recipe Recipe { get; set; }

        [Required]
        public virtual DiaryUser DiaryUser { get; set; }

        [Required]
        [Range(1.0,5.0,ErrorMessage = "Grade must be in between 1.0 and 5.0 .")]
        public double Grade { get; set; }

        public string Comment { get; set; }
    }
}