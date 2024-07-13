using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Ghak.libraries.GoogleMap.Utils;

namespace Ghak.libraries.GoogleMap;

public partial class GoogleMap : ComponentBase
{
    private DotNetObjectReference<GoogleMap>? componentRef;
    private string? mapId = Guid.NewGuid().ToString("N");

    [Parameter] public string Height { get; set; } = "400px";
    [Parameter] public string Width { get; set; } = "100%";
    [Parameter] public bool MarkMultipleLocations { get; set; }
    [Parameter] public bool AddMarkOnDoubleClick { get; set; } = true;
    [Parameter] public bool DisableDoubleClickZoom { get; set; } = true;
    [Parameter] public EventCallback<GoogleMapLocation> OnDoubleClick { get; set; }
    [Parameter] public EventCallback<GoogleMapBounds> OnBoundsChange { get; set; }
    
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (!firstRender)
            return;
        
        componentRef = DotNetObjectReference.Create(this);
        await JsRuntime.InvokeVoidAsync("initializeMap", componentRef, Configuration, mapId, new
        {
            AddMarkOnDoubleClick = AddMarkOnDoubleClick,
            MarkMultipleLocations = MarkMultipleLocations,
            DisableDoubleClickZoom = DisableDoubleClickZoom
        });
    }
    
    [JSInvokable]
    public async Task DoubleClick(double lat, double lng) 
        => await OnDoubleClick.InvokeAsync(new(lat, lng));
    
    [JSInvokable]
    public async Task BoundsChange(double neLat, double neLng, double swLat, double swLng)
        => await OnBoundsChange.InvokeAsync(new(new(neLat, neLng), new(swLat, swLng)));


    public async Task UpdateMarkers(List<GoogleMapLocation> locations, bool clearOldMarks = false) =>
        await JsRuntime.InvokeVoidAsync("updateMarkers", mapId, locations, clearOldMarks);
    
    public async Task AddMarker(GoogleMapLocation location, int? zoom = null) =>
        await JsRuntime.InvokeVoidAsync("addMarker", mapId, location, zoom);
    
    public async Task MoveMapTo(GoogleMapLocation location, int? zoom = null) =>
        await JsRuntime.InvokeVoidAsync("moveMapTo", mapId, location, zoom);
    
    public async Task ClearAllMarks() =>
        await JsRuntime.InvokeVoidAsync("clearMarkers", mapId);
    
    public async Task<GoogleMapBounds?> GetMapBounds()
    {
        try
        {
            var bounds = await JsRuntime.InvokeAsync<object>("getMapBounds", mapId);
            var boundsDict =
                JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, double>>>(
                    bounds.ToString());
            var ne = boundsDict["northeast"];
            var sw = boundsDict["southwest"];
            return new GoogleMapBounds(new GoogleMapLocation(ne["lat"], ne["lng"]),
                new GoogleMapLocation(sw["lat"], sw["lng"]));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }
}
