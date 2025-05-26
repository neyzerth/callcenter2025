public class StationListViewModel :  JsonResponse
{
    public List<Station> Stations { get; set; }
    
    public static StationListViewModel GetResponse(List<Station> stations)
    {
        StationListViewModel r = new StationListViewModel();
        r.Status = 0;
        r.Stations = stations;
        return r;
    }
}