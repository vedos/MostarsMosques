
//Angular App Module and Controller


var mapsApp = angular.module('mapsApp', ['uiGmapgoogle-maps','angularUtils.directives.dirPagination']);

mapsApp.controller('MapCtrl', function ($scope, $http) {

    var map;
    var marker;
    $scope.setCoordinates = function () {
        var lat = document.getElementById("inputLat").value;
        var lng = document.getElementById("inputLng").value;

        if (lat <= 90 && lat >= -90) {
            if (lng <= 180 && lng >= -180) { //validacija unosa
                var panPoint = new google.maps.LatLng(lat, lng);
                if (marker) {
                    marker.setPosition(panPoint);
                } else {
                    marker = new google.maps.Marker({
                        position: panPoint,
                        map: map
                    });
                }
                map.panTo(panPoint);
            }
        }
       
    }
   /* $scope.initCoordinates = function (locationID) {
        $http.get('/Administrator/Objekat/GetMarkerInfo', {
            params: {
                locationID: locationID
            }
        }).then(function (data) {
            //clear markers 
            $scope.lat = data.data.latitude;
            $scope.lng = data.data.longitude;
            //set map focus to center
        }, function () {
            alert('Error');
        });
    }*/
    $scope.initCoordinates = function (latitude,longitude) {
            //clear markers 
            $scope.lat = latitude;
            $scope.lng = longitude;
            //set map focus to center
    }
    function initialize() {
        var lat = document.getElementById("inputLat").value;
        var lng = document.getElementById("inputLng").value;
        var center;
        if(lat == "" && lng == "")
            center = { lat: 43.343898, lng: 17.807808 };
        else
            center = { lat: parseFloat(lat), lng: parseFloat(lng) };
        map = new google.maps.Map(document.getElementById('map'), {
            zoom: 16,
            center: center
        });

        

        // This event listener calls addMarker() when the map is clicked.
        google.maps.event.addListener(map, 'click', function (event) {
            var myLatLng = event.latLng;
            var lat = myLatLng.lat();
            var lng = myLatLng.lng();
            if (lng != "" && lat != "") {
                document.getElementById("inputLat").value = lat;
                document.getElementById("inputLng").value = lng;
            }
            setMarker(event.latLng, map);
        });

        // Add a marker at the center of the map.
        setMarker(center, map);
    }




    // Adds a marker to the map.
    function setMarker(location,map) {
        // Add the marker at the clicked location, and add the next-available label
        // from the array of alphabetical characters.
        if (marker) {
            marker.setPosition(location);
        } else {
            marker = new google.maps.Marker({
                position: location,
                map: map
            });
        }
    }

    google.maps.event.addDomListener(window, 'load', initialize);


});


mapsApp.controller('MapCtrl2', function ($scope,$http) {
    //this is for default map focus when load first time
    var mostar = { latitude: 43.343898, longitude: 17.807808 };
    $scope.map = { center: mostar, zoom: 16 }
 
    $scope.markers = [];
    $scope.locations = [];
    $scope.selectedMedzlis = null;
    $scope.medzlis = [];
    $scope.currentPage = 1;
    $scope.pageSize = 5;
    $scope.TotalLocationsCount = 0;
    $scope.inputSearch = "";
    

    //Poplate all Medzlisi
    $http.get('/Administrator/Objekat/GetMedzlisi').then(function (data) {
        $scope.medzlis = data.data;
    }, function () {
        alert('Error');
    });

    //Populate all location
    $http.get('/Administrator/Objekat/GetAllLocation', {
        params: {
            page: $scope.currentPage
        }
    }).then(function (data) {
        $scope.locations = data.data.locationsPaged;

        $scope.TotalLocationsCount =data.data.TotalLocationsCount;
    }, function () {
        alert('Error');
    });

    $scope.onMedzlisSelected = function () {
        if ($scope.selectedMedzlis != null) {
            $http.get('/Administrator/Objekat/SelectObjektiByMedzlisId', {
                params: {
                    medzlisId: $scope.selectedMedzlis
                }
            }).then(function (data) {
                $scope.locations = data.data.locationsPaged;
                $scope.TotalLocationsCount = data.data.TotalLocationsCount;
            }, function () {
                alert('Error');
            });
        } else {
            $http.get('/Administrator/Objekat/GetAllLocation').then(function (data) {
                $scope.locations = data.data.locationsPaged;
                $scope.TotalLocationsCount = data.data.TotalLocationsCount;
            }, function () {
                alert('Error');
            });
        }
    }
 
    //get marker info
    $scope.ShowLocation = function (locationID) {
        $http.get('/Administrator/Objekat/GetMarkerInfo', {
            params: {
                locationID: locationID
            }
        }).then(function (data) {
            //clear markers 
            $scope.markers = [];
            $scope.markers.push({
                id: data.data.Id,
                coords: { latitude: data.data.latitude, longitude: data.data.longitude },
                naziv: data.data.Naziv,
                //address: data.data.Address,
                //image : data.data.ImagePath
            });
 
            //set map focus to center
            $scope.map.center.latitude = data.data.latitude;
            $scope.map.center.longitude = data.data.longitude;
 
        }, function () {
            alert('Error');
        });
    }

    $scope.Search = function (inputSearch)
    {
        if (inputSearch != "") {
            $http.get('/Administrator/Objekat/SearchLocations', {
                params: {
                    inputSearch: inputSearch
                }
            }).then(function (data) {
                $scope.locations = data.data.locationsPaged;
                $scope.TotalLocationsCount = data.data.TotalLocationsCount;
            }, function () {
                alert('Error');
            });
        } else
        {
            $http.get('/Administrator/Objekat/GetAllLocation').then(function (data) {
                $scope.locations = data.data.locationsPaged;
                $scope.TotalLocationsCount = data.data.TotalLocationsCount;
            }, function () {
                alert('Error');
            });
        }
    }
    
    $scope.ShowAllMarkers = function () {
        if ($scope.selectedMedzlis == null) {
            $http.get('/Administrator/Objekat/GetAllMarkersInfo').then(function (data) {
                //clear markers 
                $scope.markers = [];

                //set results
                angular.forEach(data.data, function (value, key) {

                    $scope.markers.push({
                        id: value.Id,
                        coords: { latitude: value.latitude, longitude: value.longitude },
                        naziv: value.Naziv,
                    });
                });

            }, function () {
                alert('Error');
            });

        } else {
            $http.get('/Administrator/Objekat/GetMarkersByMedzlisId', {
                params: {
                    medzlisId: $scope.selectedMedzlis
                }
            }).then(function (data) {
                //clear markers 
                $scope.markers = [];

                //set results
                angular.forEach(data.data, function (value, key) {

                    $scope.markers.push({
                        id: value.Id,
                        coords: { latitude: value.latitude, longitude: value.longitude },
                        naziv: value.Naziv,
                    });
                });
            });
        }

    }
 
    //Show / Hide marker on map
    $scope.windowOptions = {
        show: true
    };

    $scope.pageChangeHandler = function (num) {
        if ($scope.selectedMedzlis != null) {
            $http.get('/Administrator/Objekat/SelectObjektiByMedzlisId', {
                params: {
                    medzlisId: $scope.selectedMedzlis,
                    page: $scope.currentPage
                }
            }).then(function (data) {
                $scope.locations = data.data.locationsPaged;
                $scope.TotalLocationsCount = data.data.TotalLocationsCount;
            }, function () {
                alert('Error');
            });

        } else if ($scope.inputSearch != "")
        {
            $http.get('/Administrator/Objekat/SearchLocations', {
                params: {
                    inputSearch: inputSearch,
                    page: $scope.currentPage
                }
            }).then(function (data) {
                $scope.locations = data.data.locationsPaged;
                $scope.TotalLocationsCount = data.data.TotalLocationsCount;
            }, function () {
                alert('Error');
            });
        }
        else {
            $http.get('/Administrator/Objekat/GetAllLocation', {
                params: {
                    page: $scope.currentPage
                }
            }).then(function (data) {
                $scope.locations = data.data.locationsPaged;
                $scope.TotalLocationsCount = data.data.TotalLocationsCount;
            }, function () {
                alert('Error');
            });
        }

    };

});
