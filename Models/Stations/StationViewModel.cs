public class StationViewModel
{
    public int Status { get; set; }
    public Station Station { get; set; }
    
    public static StationViewModel GetResponse(Station station)
    {
        StationViewModel r = new StationViewModel();
        r.Status = 0;
        r.Station = station;
        return r;
    }
    
}