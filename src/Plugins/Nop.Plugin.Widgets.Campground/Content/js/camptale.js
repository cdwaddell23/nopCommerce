/*************************************************************
 * 
 * Mapping JS 
 * 
 *************************************************************/
var map;
var campmarker;
var service;
var mapZoom;
var detailWindow;
var mapType = 'terrain';
var related_markers = new Array();
var locations = new Array();
var contentString = new Array();
//var campgroundLatLng = { lat: 41.850706, lng: -90.1930883 }; //The first place I called home.
//var campgroundLatLng = {lat: 37.0902, lng: -95.7129 }; //center US
var campgroundLatLng = { lat: 39.494486, lng: -104.8876487 }; //Camptale home office.

function showSmallSearchAd() {
    $('#idSearchLargeAd').hide();
    $('#idSearchSmallAd').show();
}

function showLargeSearchAd() {
    $('#idSearchLargeAd').show();
    $('#idSearchSmallAd').hide();
}

/***********************************************************************
 * 
 * Mapping Utility functions
 * 
 ***********************************************************************/

function handleEvent(event) {
    map.setCenter(campmarker.getPosition());
    document.getElementById('mapLatitude').value = event.latLng.lat();
    document.getElementById('mapLongitude').value = event.latLng.lng();
}

function fitMapBounds() {
    var bounds = new google.maps.LatLngBounds();
    for (var i = 0; i < related_markers.length; i++) {
        bounds.extend(related_markers[i].getPosition());
    }

    map.fitBounds(bounds, 10);
}

function performSearch() {
    var restaurant = {
        bounds: map.getBounds(),
        type: ['restaurant']
    };
    service.nearbySearch(restaurant, Restaurant);
    var gas = {
        bounds: map.getBounds(),
        type: ['gas_station']
    };
    service.nearbySearch(gas, Gas);
}

function callback(results, status, locationMark) {
    if (status !== google.maps.places.PlacesServiceStatus.OK) {
        return;
    }
    for (var i = 0, result; result = results[i]; i++) {
        addMarker(result, locationMark);
    }
}

function addMarker(place, locationMark) {
    var marker = new google.maps.Marker({
        map: map,
        position: place.geometry.location,
        icon: {
            url: locationMark,
            anchor: new google.maps.Point(25, 25),
            scaledSize: new google.maps.Size(25, 25)
        }
    });
    detailWindow = new google.maps.InfoWindow()
    google.maps.event.addListener(marker, 'click', function () {
        service.getDetails(place, function (result, status) {
            if (status !== google.maps.places.PlacesServiceStatus.OK) {
                console.error(status);
                return;
            }
            detailWindow.setContent(result.name);
            detailWindow.open(map, marker);
        });
    });
}

function addPlacesMarker(place) {
    detailWindow = new google.maps.InfoWindow()
    var marker = new google.maps.Marker({
        map: map,
        position: place.geometry.location,
        icon: {
            url: 'https://developers.google.com/maps/documentation/javascript/images/circle.png',
            anchor: new google.maps.Point(10, 10),
            scaledSize: new google.maps.Size(10, 17)
        }
    });

    google.maps.event.addListener(marker, 'click', function () {
        service.getDetails(place, function (result, status) {
            if (status !== google.maps.places.PlacesServiceStatus.OK) {
                console.error(status);
                return;
            }
            detailWindow.setContent(result.name);
            detailWindow.open(map, marker);
        });
    });
}

function Restaurant(results, status) {
    var locationMark = '/themes/nopelectro/content/images/maps/food.png';
    callback(results, status, locationMark)
}
function Gas(results, status) {
    var locationMark = '/themes/nopelectro/content/images/maps/fuel.png';
    callback(results, status, locationMark)
}

/***********************************************************************
 * 
 * Map for campground host apply page
 * 
 ***********************************************************************/
function initAddressMap() {
    var imgDestinationMarker = {
        url: '//www.camptale.com/plugins/campgrounds/content/images/maps/camping-green-logo.png',
        // This marker is 20 pixels wide by 32 pixels high.
        size: new google.maps.Size(35, 35),
        // The origin for this image is (0, 0).
        origin: new google.maps.Point(0, 0),
        // The anchor for this image is the base of the flagpole at (0, 32).
        anchor: new google.maps.Point(0, 32)
    };

    map = new google.maps.Map(document.getElementById('map'), {
        center: campgroundLatLng,
        zoom: mapZoom,
        mapTypeId: mapType.toLowerCase(),
        styles: [{
            stylers: [{ visibility: 'simplified' }]
        }, {
            elementType: 'labels',
            stylers: [{ visibility: 'on' }]
        }]
    });
    campmarker = new google.maps.Marker({
        position: campgroundLatLng,
        draggable: true,
        map: map,
        title: '',
        icon: imgDestinationMarker,
    });

    //campmarker.addListener('drag', handleEvent);
    campmarker.addListener('dragend', handleEvent);
    campmarker.addListener('changed', handleEvent);
}


/***********************************************************************
 * 
 * Map for campground overview page
 * 
 ***********************************************************************/
function initOverviewMap() {

    detailWindow = new google.maps.InfoWindow()
    var imgMarker = {
        url: '//www.camptale.com/plugins/campgrounds/content/images/maps/camping-logo.png',
        // This marker is 20 pixels wide by 32 pixels high.
        size: new google.maps.Size(35, 35),
        // The origin for this image is (0, 0).
        origin: new google.maps.Point(0, 0),
        // The anchor for this image is the base of the flagpole at (0, 32).
        anchor: new google.maps.Point(0, 32)
    };

    if ($(window).width() > 739) {
        map = new google.maps.Map(document.getElementById('map_side'), {
            zoom: mapZoom,
            center: campgroundLatLng,
            mapTypeId: mapType.toLowerCase(),
        });
        $("#topMap").hide();
        $("#map_side").show();
    }
    else {
        map = new google.maps.Map(document.getElementById('map_top'), {
            zoom: mapZoom,
            center: campgroundLatLng,
            mapTypeId: mapType.toLowerCase(),
        });
        $("#topMap").show();
        $("#map_side").hide();
    }


    for (var i = 0; i < locations.length; i++) {
        contentString[i] = '<div id="content-' + locations[i][4] + '">' +
            '<div id="siteNotice-' + locations[i][4] + '">' +
            '</div>' +
            '<h6 id="firstHeading-' + locations[i][4] + '" class="firstHeading">' + locations[i][0] + '</h6>' +
            '<div id="bodyContent-' + locations[i][4] + '">' +
            '<p>' + locations[i][1] + '</p>' +
            '</div>' +
            '</div>';

        var maplatlng = { lat: locations[i][2], lng: locations[i][3] }

        marker = new google.maps.Marker({
            position: maplatlng,
            label: {
                text: (i + 1).toString(),
                color: "yellow",
                fontSize: "15px",
                fontWeight: "bold",
            },
            labelOrigin: new google.maps.Point(9, 9),
            map: map,
            //draggable: true,
            //animation: google.maps.Animation.DROP,
            title: locations[i][0],
            icon: imgMarker,
            zIndex: locations[i][4]
        });

        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                var elem = document.getElementById("campground-summary-card-" + locations[i][4]);
                var cloneElem = elem.cloneNode(true);
                var elemMap = cloneElem.getElementsByClassName("campground_map_" + locations[i][4]);
                elem.style.backgroundColor = '#e3e3e3';
                elemMap["0"].style.display = 'none';
                $("campground_map_" + locations[i][4]).hide();
                detailWindow.setContent(cloneElem);
                detailWindow.open(map, marker);
            }
        })(marker, i));

        google.maps.event.addListener(detailWindow, 'closeclick', (function (detailWindow, i) {
            return function () {
                var elem = document.getElementById(detailWindow.content.id);
                elem.removeAttribute("style");
                marker.setMap(null);
            }
        })(detailWindow, i));

        related_markers.push(marker);
    }

    fitMapBounds();
}


/***********************************************************************
 * 
 * Map for campground detail page with nearby locations
 * 
 ***********************************************************************/
function initDetailMap(campName, campDesc) {

    detailWindow = new google.maps.InfoWindow()
    var imgDestinationMarker = {
        url: '//www.camptale.com/plugins/campgrounds/content/images/maps/camping-green-logo.png',
        // This marker is 20 pixels wide by 32 pixels high.
        size: new google.maps.Size(35, 35),
        // The origin for this image is (0, 0).
        origin: new google.maps.Point(0, 0),
        // The anchor for this image is the base of the flagpole at (0, 32).
        anchor: new google.maps.Point(0, 32)
    };
    var imgNearbyMarker = {
        url: '//www.camptale.com/plugins/campgrounds/content/images/maps/camping-tan-logo.png',
        // This marker is 20 pixels wide by 32 pixels high.
        size: new google.maps.Size(35, 35),
        // The origin for this image is (0, 0).
        origin: new google.maps.Point(0, 0),
        // The anchor for this image is the base of the flagpole at (0, 32).
        anchor: new google.maps.Point(0, 32)
    };
    map = new google.maps.Map(document.getElementById('map'), {
        center: campgroundLatLng,
        zoom: mapZoom,
        mapTypeId: mapType.toLowerCase(),
        styles: [{
            stylers: [{ visibility: 'simplified' }]
        }, {
            elementType: 'labels',
            stylers: [{ visibility: 'on' }]
        }]
    });

    var contentString = '<div id="content">' +
        '<div id="siteNotice">' +
        '</div>' +
        '<h6 id="firstHeading" class="firstHeading">' + campName + '</h6>' +
        '<div id="bodyContent">' +
        '<p>' + campDesc + '</p>' +
        '</div>' +
        '</div>';
    var campwindow = new google.maps.InfoWindow({
        content: contentString
    });

    service = new google.maps.places.PlacesService(map);

    // The idle event is a debounced event, so we can query & listen without
    // throwing too many requests at the server.
    map.addListener('idle', performSearch);

    for (var i = 0; i < locations.length; i++) {
        contentString[i] = '<div id="content-' + locations[i][4] + '">' +
            '<div id="siteNotice-' + locations[i][4] + '">' +
            '</div>' +
            '<h6 id="firstHeading-' + locations[i][4] + '" class="firstHeading">' + locations[i][0] + '</h6>' +
            '<div id="bodyContent-' + locations[i][4] + '">' +
            '<p>' + locations[i][1] + '</p>' +
            '</div>' +
            '</div>';

        var maplatlng = { lat: locations[i][2], lng: locations[i][3] }

        marker = new google.maps.Marker({
            position: maplatlng,
            map: map,
            title: locations[i][0],
            icon: imgNearbyMarker,
            zIndex: locations[i][4]
        });

        google.maps.event.addListener(marker, 'click', (function (marker, i) {
            return function () {
                var elem = document.getElementById("campground-summary-card-" + locations[i][4]);
                var elemMap = elem.getElementsByClassName("campground_map_" + locations[i][4]);
                //elemMap["0"].style.display = 'none';
                $("campground_map_" + locations[i][4]).hide();
                detailWindow.setContent(elem);
                detailWindow.open(map, marker);
            }
        })(marker, i));

        related_markers.push(marker);
    }

    var campmarker = new google.maps.Marker({
        position: campgroundLatLng,
        map: map,
        title: campName,
        icon: imgDestinationMarker,
    });

    campmarker.addListener('click', function () {
        campwindow.open(map, marker);
    });

    //fitMapBounds();
}

