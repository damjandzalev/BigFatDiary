using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigFatDiary.Models.Data
{
    public class DiaryUser
    {
        [Key]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }

        [Display(Name = "Gender")]
        public Gender Gender { get; set; }
    }
}