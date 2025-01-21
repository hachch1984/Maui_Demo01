namespace Dto.Ges.Product
{


    public class UpdateProd
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
        public int CategoryId { get; set; }
    }

}
