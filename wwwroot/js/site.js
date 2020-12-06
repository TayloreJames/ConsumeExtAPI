// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

use 

const triangleCoords = [
    { lat: 25.774, lng: -80.19 },
    { lat: 18.466, lng: -66.118 },
    { lat: 32.321, lng: -64.757 },
];
const bermudaTriangle = new google.maps.Polygon({
    paths: triangleCoords,
});

var myPolygon = new google.maps.Polygon({
    paths: [
        new google.maps.LatLng(25.774, -80.190),
        new google.maps.LatLng(18.466, -66.118),
        new google.maps.LatLng(32.321, -64.757)
    ]
});

var testPolygon = new google.maps.Polygon()