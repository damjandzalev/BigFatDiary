using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BigFatDiary.Models.View
{
    public class ListRecipes
    {
        public string SearchText { get; set; }

        public bool IsSearch { get; set; }

        public List<DetailsRecipe> Recipes { get; set; }
    }
}