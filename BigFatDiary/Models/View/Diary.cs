using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigFatDiary.Models.View
{
    public class Diary
    {
        public List<DiaryDayDetails> DayDetails { get; set; }
        public bool hasMeasurements { get; set; }
    }
}