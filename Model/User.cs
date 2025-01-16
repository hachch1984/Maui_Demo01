namespace Model
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserDocumentValue { get; set; }

        #region
        public int UserDocumentTypeId { get; set; }
        public UserDocumentType UserDocumentType { get; set; }
        #endregion
    }
}
