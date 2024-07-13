namespace Ghak.libraries.GoogleMap.Utils;

public class GoogleMapsConfiguration
{
    public string Url { get; set; } = "https://maps.googleapis.com/maps/api/js";
    public required string ApiKey { get; set; }
    public bool WithLog { get; set; } = true;
    public GoogleMapLocation InitLocation { get; set; } = new(23.5880, 58.3829);
    public int InitZoom { get; set; } = 11;
    public GoogleMapTypeId MapTypeId { get; set; } = GoogleMapTypeId.Satellite;
    public string GoogleMapType => MapTypeId.ToString().ToLower();
}
