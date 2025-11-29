window.blazorMaps = (function () {
    let map = null;
    const markers = {};
    let dotNetRef = null;

    function initMap(elementId, options) {
        const el = document.getElementById(elementId);
        if (!el) return console.error("Map element not found:", elementId);
        map = new google.maps.Map(el, options);
    }

    function colorIcon(hex) {
        return {
            url: `data:image/svg+xml;utf8,
                <svg xmlns='http://www.w3.org/2000/svg' width='40' height='40'>
                    <circle cx='20' cy='20' r='14' fill='${hex}' />
                </svg>`,
            scaledSize: new google.maps.Size(40, 40)
        };
    }

    function addMarker(marker) {
        const m = new google.maps.Marker({
            map,
            position: { lat: marker.lat, lng: marker.lng },
            title: marker.title,
            icon: colorIcon(marker.color)
        });

        m.addListener('click', function () {
            if (dotNetRef) {
                dotNetRef.invokeMethodAsync('OnMarkerClicked', marker.id);
            }
        });

        markers[marker.id] = m;
    }

    function registerDotNet(dotNetObject) {
        dotNetRef = dotNetObject;
    }

    return {
        initMap,
        addMarker,
        registerDotNet
    };
})();
