namespace Dto
{
    public class Token_Dto_For_ShowInformation
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime Creation { get; set; }
    }

    public class Token_Dto_For_Create
    {
        public int UserDocumentTypeId { get; set; }
        public string UserDocumentValue { get; set; }
        public string Password { get; set; }
    }


}
