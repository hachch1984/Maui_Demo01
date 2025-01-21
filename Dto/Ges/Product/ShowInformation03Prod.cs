namespace Dto.Ges.Product
{

    /// <summary>
    /// Id, Name, Description, Price, Active
    /// </summary>
    public class ShowInformation03Prod
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }
    }
}
