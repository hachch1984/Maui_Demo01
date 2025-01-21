using AutoMapper; 

namespace Dto.Ges.Product
{
    public class _Profile : Profile
    {
        public _Profile()
        { 
            CreateMap<Model.Product,Dto.Ges.Product. AddProd>().ReverseMap();
            CreateMap<Model.Product, Dto.Ges.Product.UpdateProd>().ReverseMap();
        }
    }




}
