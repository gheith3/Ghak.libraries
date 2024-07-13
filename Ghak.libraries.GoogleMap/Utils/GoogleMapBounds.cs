namespace Ghak.libraries.GoogleMap.Utils;

public class GoogleMapBounds
{
    public GoogleMapLocation NorthEast { get; set; }
    public GoogleMapLocation SouthWest { get; set; }

    public GoogleMapBounds(GoogleMapLocation northEast, GoogleMapLocation southWest)
    {
        NorthEast = northEast;
        SouthWest = southWest;
    }
}
