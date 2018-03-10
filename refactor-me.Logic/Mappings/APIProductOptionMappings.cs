using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using refactor_me.Models;
using refactor_me.ViewModels;

namespace refactor_me.Logic.Mappings
{
    public class APIProductOptionMappings : Profile
    {
        public APIProductOptionMappings()
        {
            CreateMap<ProductOption, ProductOptionView>();
        }
    }
}
