namespace Ghak.libraries.GoogleMap.Utils;

public class GoogleMapLocation
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string? Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string? InfoWindowHtml { get; set; }

    public GoogleMapLocation()
    {
    }

    public GoogleMapLocation(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
    
    public GoogleMapLocation(string id, string name, double latitude, double longitude, string? infoWindowHtml = null)
    {
        Id = id;
        Latitude = latitude;
        Longitude = longitude;
        Name = name;
        InfoWindowHtml = infoWindowHtml;
    }
}
