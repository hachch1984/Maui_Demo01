namespace Model
{
    public class Message_ToSend
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
