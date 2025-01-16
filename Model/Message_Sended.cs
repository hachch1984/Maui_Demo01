namespace Model
{
    public class Message_Sended
    {
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        public string Message { get; set; }



        #region
        public long UserId { get; set; }
        public User User { get; set; }
        #endregion
    }
}
