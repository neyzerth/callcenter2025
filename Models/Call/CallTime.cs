public class CallTime
{
    public DateTime DateTimeReceived { get; set; }
    public DateTime DateTimeAnswered { get; set; }
    public DateTime DateTimeEnded { get; set; }
    public TimeSpan TimeInQueue { get; set; }
    public TimeSpan TimeInCall { get; set; }
}