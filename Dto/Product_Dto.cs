using AutoMapper;
using Model;

namespace Dto
{
    public class Product_Dto : Profile
    {
        public Product_Dto()
        {
            CreateMap<Product, Product_Dto_For_ShowInformation01>().ReverseMap();
            CreateMap<Product, Product_Dto_For_ShowInformation02>().ReverseMap();
            CreateMap<Product, Product_Dto_For_Add>().ReverseMap();
            CreateMap<Product, Product_Dto_For_Update>().ReverseMap();
        }
    }

    /// <summary>
    /// Id, Name, Description, Price, Active, CategoryId
    /// </summary>
    public class Product_Dto_For_ShowInformation01
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public Category_Dto_For_ShowInformation01 Category { get; set; }
    }
    /// <summary>
    /// Id, Name
    /// </summary>
    public class Product_Dto_For_ShowInformation02
    {
        public long Id { get; set; }
        public string Name { get; set; }

    }
    /// <summary>
    /// Id, Name, Description, Price, Active
    /// </summary>
    public class Product_Dto_For_ShowInformation03
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
    }

    public class Product_Dto_For_Add
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public int CategoryId { get; set; }
    }

    public class Product_Dto_For_Update
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public int CategoryId { get; set; }
    }

    public class Product_Dto_For_Update_Active
    {
        public long Id { get; set; }
        public bool Active { get; set; }
    }

    public class Product_Dto_For_Update_Price
    {
        public long Id { get; set; }
        public decimal Price { get; set; }
    }
}
