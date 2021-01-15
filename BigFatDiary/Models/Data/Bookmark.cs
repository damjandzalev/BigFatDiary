using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigFatDiary.Models.Data
{
    public class Bookmark
    {
        [Key]
        public long Id { get; set; }

        public virtual DiaryUser DiaryUser { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}