using AutoMapper;

namespace Dto.Ges.Category
{
    public class _Profile : Profile
    {
        public _Profile()
        { 

            CreateMap<Model.Category, AddCat>().ReverseMap();
            CreateMap<Model.Category, UpdateCat>().ReverseMap();
        }
    }










}
