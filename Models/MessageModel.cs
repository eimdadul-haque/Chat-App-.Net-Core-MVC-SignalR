public class MessageModel
{
    public int Id { get; set; }
    public string?  msgBody { get; set; }
    public string?  sender { get; set; }
    public string?  reciver { get; set; }
    public DateTime when { get; set; } = DateTime.Now;
}