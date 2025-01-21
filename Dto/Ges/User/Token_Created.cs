namespace Dto.Ges.User
{
    public class Token_Created
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime Creation { get; set; }
    }
}
