using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BigFatDiary.Models.Data.Enumerations
{
    public enum MeasurementUnit
    {
        [Display(Name = "Gram")]
        GRAM,
        [Display(Name = "Piece")]
        PIECE,
        [Display(Name = "Milliliter")]
        MILLILITER
    }
}