namespace Model
{
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Active { get; set; }

        #region
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        #endregion
    }
}
