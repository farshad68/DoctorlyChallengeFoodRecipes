using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Webservices.Models;
using Webservices.ViewModel;

namespace Webservices
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            //CreateMap<Recipe, RecipeViewModel>();
            //CreateMap<RecipeViewModel, Recipe>();
        }
    }
}
