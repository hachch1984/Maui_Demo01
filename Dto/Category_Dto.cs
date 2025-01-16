using AutoMapper;
using Model;

namespace Dto
{
    public class Category_Dto : Profile
    {
        public Category_Dto()
        {
            CreateMap<Category, Category_Dto_For_ShowInformation01>().ReverseMap();
            CreateMap<Category, Category_Dto_For_ShowInformation02>().ReverseMap();
            CreateMap<Category, Category_Dto_For_ShowInformation03>().ReverseMap();

            CreateMap<Category, Category_Dto_For_Add>().ReverseMap();
            CreateMap<Category, Category_Dto_For_Update>().ReverseMap();
        }
    }

    /// <summary>
    /// Id, Name
    /// </summary>
    public class Category_Dto_For_ShowInformation01
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    /// <summary>
    /// Id, Name, Description
    /// </summary>
    public class Category_Dto_For_ShowInformation02
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    /// <summary>
    /// Id, Name, Description, Active
    /// </summary>
    public class Category_Dto_For_ShowInformation03
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }

    public class Category_Dto_For_Add
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }

    public class Category_Dto_For_Update
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
    }

    public class Category_Dto_For_Update_Active
    {
        public int Id { get; set; }
        public bool Active { get; set; }
    }

}
