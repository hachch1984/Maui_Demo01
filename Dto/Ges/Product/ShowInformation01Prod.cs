 

namespace Dto.Ges.Product
{
    /// <summary>
    /// Id, Name, Description, Price, Active, CategoryId
    /// </summary>
    public class ShowInformation01Prod
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public Category.ShowInformation1Cat Category { get; set; }
    }
}
