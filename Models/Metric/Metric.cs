public class Metric
{
    public string Value { get; set; }
    public string Status { get; set; }

    public Metric()
    {
        Value = "00:00";
        Status = MetricStatus.GOOD.ToString();
    }
    public Metric(string value, MetricStatus status)
    {
        Value = value;
        Status = status.ToString();
    }
}