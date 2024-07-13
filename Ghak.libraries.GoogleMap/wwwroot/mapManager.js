let maps = {};
let config;

document.addEventListener('DOMContentLoaded', function () {
    console.log("DOM fully loaded and parsed");

    DotNet.attachReviver((key, value) => {
        if (value && typeof value === 'object' && value.hasOwnProperty('__dotNetObject')) {
            return {
                invokeMethodAsync: function (methodName, ...args) {
                    return DotNet.invokeMethodAsync(value.__dotNetObject, methodName, ...args);
                }
            };
        }
        return value;
    });
});

function WriteLog(message) {
    try {
        if (config.withLog)
            console.log(`Google Map Component => ${message}`);
    }catch (e) {
        console.error("Error writing log:", e.message);
        console.error("Log:", message);
    }
}
function WriteErrorLog (message) {
    try {
        if (config.withLog)
            console.error(`Google Map Component => ${message}`);
    }catch (e) {
        console.error("Error writing error log:", e.message);
        console.error("Error log:", message);
    }
}

window.initializeMap = function (_dotNetReference, _config, _mapId, _componentConfig) {

    try {
        config = _config;

        maps[_mapId] = {
            map: null,
            dotNetReference: _dotNetReference,
            mapId: _mapId,
            componentConfig: _componentConfig,
            markers: [],
            currentInfoWindow: null,
        };

        WriteLog("config set:", config);
        loadGoogleMapsScript(_mapId);
    }catch (e) {
        WriteErrorLog("Error initializing map:", e.message);
    }
}

function loadGoogleMapsScript(mapId) {
    const isScriptLoaded = document.querySelector('script[src^="https://maps.googleapis.com/maps/api/js?key="]');
    if (isScriptLoaded) {
        if (typeof google !== 'undefined' && google.maps) {
            window.initMap(mapId);
        } else {
            isScriptLoaded.addEventListener('load', () => window.initMap(mapId));
        }
        return;
    }

    WriteLog("Loading Google Maps script");
    const script = document.createElement('script');
    script.src = `${config.url}?key=${config.apiKey}`;
    script.async = true;
    script.defer = true;
    script.onload = () => window.initMap(mapId);
    document.head.appendChild(script);
}

window.initMap = function (mapId) {
    WriteLog("initMap called");
    try {
        
        var map = new google.maps.Map(document.getElementById(`${mapId}`), {
            center: { lat: config.initLocation.latitude, lng: config.initLocation.longitude },
            zoom: config.initZoom,
            disableDoubleClickZoom: maps[mapId].componentConfig.disableDoubleClickZoom,
            mapTypeId: config.googleMapType
        });

        WriteLog("Map initialized");

        // Wait for the map to be idle before adding the listener
        google.maps.event.addListenerOnce(map, 'idle', function () {
            // Correct usage of debounce with a function argument
            map.addListener("bounds_changed", debounce(() => onBoundsChange(mapId), 300));
            //fetchCityData(); // Initial fetch

            map.addListener("dblclick", function (e) {
                onDoubleClick(e.latLng, mapId);
            });
        });

        maps[mapId].map = map;

    } catch (error) {
        console.error("Error initializing map:", error);
    }
}


function debounce(func, delay) {
    let timeoutId;
    return function() {
        const context = this;
        // Using the arguments object to capture the passed arguments
        const args = arguments;
        // Check if func is a function
        if (typeof func !== 'function') {
            console.error('Debounce function argument is not a function:', func);
            return;
        }
        clearTimeout(timeoutId);
        timeoutId = setTimeout(() => func.apply(context, args), delay);
    };
}



async function onBoundsChange(mapId) {
    try {
        if (!maps[mapId].dotNetReference || !maps[mapId].map) {
            WriteErrorLog("DotNet reference or map not initialized");
            return;
        }

        const bounds = maps[mapId].map.getBounds();
        if (!bounds) {
            WriteErrorLog("Map bounds not available yet");
            return;
        }
        const ne = bounds.getNorthEast();
        const sw = bounds.getSouthWest();

        WriteLog("Invoking UpdateMarkers");
        await maps[mapId].dotNetReference.invokeMethodAsync('BoundsChange', ne.lat(), ne.lng(), sw.lat(), sw.lng());
    } catch (error) {
        WriteErrorLog("Error details:" + error.message);
        WriteErrorLog("Stack trace:" + error.stack);
    }
}

async function onDoubleClick(latLng, mapId) {

    try {
        if (!maps[mapId].componentConfig.addMarkOnDoubleClick){
            return;
        }

        WriteLog("Double click detected at", latLng.toString());
        let placeName = "New Location";

        if (!maps[mapId].componentConfig.markMultipleLocations){
            clearMarkers(mapId);
        }

        const marker = new google.maps.Marker({
            position: latLng,
            map: maps[mapId].map
        });

        WriteLog("Marker added to map" + latLng);
        maps[mapId].markers.push(marker);

        // Send location details to C#
        if (maps[mapId].dotNetReference) {
            maps[mapId].dotNetReference.invokeMethodAsync('DoubleClick', latLng.lat(), latLng.lng())
                .then(() => WriteLog("Location details sent to C#"))
                .catch(error => WriteErrorLog("Error sending location details to C#:", error));
        } else {
            WriteErrorLog("DotNet reference not initialized");
        }
    }catch (e) {
        WriteErrorLog("Error onDoubleClick:", e.message);
    }
}


window.updateMarkers = function(mapId, locations, clearOldMarkers = false) {

    WriteLog("updateMarkers called");
    if(clearOldMarkers){
        clearMarkers(mapId);
    }
    //check if locations is null or empty
    if (locations == null || locations.length == 0)
        return;

    locations.forEach(location => addMarker(mapId, location));
}



window.addMarker = function(mapId, location, zoom = null) {
    try {
        const marker = new google.maps.Marker({
            position: {lat: location.latitude, lng: location.longitude},
            map: maps[mapId].map,
            title: location.name
        });

        marker.addListener('click', () => openInfoWindow(mapId, location, marker));
        maps[mapId].markers.push(marker);
        if (zoom != null){
            maps[mapId].map.setCenter(new google.maps.LatLng(location.latitude, location.longitude));
            maps[mapId].map.setZoom(zoom);
        }

    }catch (error) {
        WriteErrorLog("Error adding marker: ", error.message);
    }
}


function openInfoWindow(mapId, location, marker) {

    try {
        //check if location.infoWindowHtml has value before opening info window
        if (location.infoWindowHtml == null || location.infoWindowHtml == "") {
            return
        }

        if (maps[mapId].currentInfoWindow) {
            maps[mapId].currentInfoWindow.close(maps[mapId].map);
            maps[mapId].currentInfoWindow = null;
        }

        maps[mapId].currentInfoWindow = new google.maps.InfoWindow({ content: location.infoWindowHtml });
        maps[mapId].currentInfoWindow.open(maps[mapId].map, marker);
        WriteLog("Info window opened");
    }catch (error) {
        WriteErrorLog("Error opening info window: ", error.message);
    }
}




window.moveMapTo = function(mapId, location, zoom) {
    try {

        const latLng = new google.maps.LatLng(location.latitude, location.longitude);
        maps[mapId].map.setCenter(latLng);

        if (zoom != null){
            maps[mapId].map.setZoom(zoom);
        }
        maps[mapId].map.setCenter(latLng);
    }catch (error) {
        WriteErrorLog("Error moving map to location: ", error.message);
    }
}

window.getMapBounds = function(mapId) {
    if (!maps[mapId].map) {
        console.error("Map not initialized");
        return null;
    }

    const bounds = maps[mapId].map.getBounds();
    const ne = bounds.getNorthEast();
    const sw = bounds.getSouthWest();
    return JSON.stringify({
        northeast: { lat: ne.lat(), lng: ne.lng() },
        southwest: { lat: sw.lat(), lng: sw.lng() }
    });
}

window.clearMarkers = function (mapId) {
    try {
        if (maps[mapId].markers.length > 0) {
            maps[mapId].markers.forEach(marker => marker.setMap(null));
            maps[mapId].markers = [];
        }
    }catch (error) {
        WriteErrorLog("Error clearing markers: ", error.message);
    }
}

window.onerror = function(message, source, lineno, colno, error) {
    WriteErrorLog("Global error caught:" + message);
    WriteErrorLog("Source:" + source);
    WriteErrorLog("Line:" + lineno);
    WriteErrorLog("Column:" + colno);
    WriteErrorLog("Error object:" + error);
    return false;
};