import './Map.css';
import { useCallback, useEffect, useState } from 'react';
import { MapContainer, TileLayer, FeatureGroup, Polygon, Polyline, useMap } from 'react-leaflet';
import osm from './osm-providers';
import 'leaflet/dist/leaflet.css';
import 'leaflet-draw/dist/leaflet.draw.css';
import { useStore } from '../../stores/store';
import { LatLngExpression } from 'leaflet';
import { observer } from 'mobx-react-lite';
import LoadingComponent from '../../layout/LoadingComponent';
import { useNavigate } from 'react-router-dom';
import { EditControl } from 'react-leaflet-draw';
//import { v4 as uuid } from "uuid";
//import agent from '../../api/agent';
import { Koordinata } from '../../modules/Koordinata';
//Sluzi samo za brisanje podrucja sa mape
const MapLayersManager = () => {
  const map = useMap();
  const { areaStore, routStore, tourStore } = useStore();
  const { isCreatingArea, getAreas, isSelected } = areaStore;
  const { isCreatingRout } = routStore;
  const { showRout } = tourStore;

  const removeAllPolygons = useCallback(() => {
    map.eachLayer((layer) => {
      if (layer instanceof Polygon) {
        map.removeLayer(layer);
      }
    });
  }, [map]);

  useEffect(() => {
    if (!isCreatingArea && !isCreatingRout && !showRout && !isSelected && getAreas.length === 0) {
      removeAllPolygons();
    }
  }, [isCreatingArea, isCreatingRout, showRout, isSelected, getAreas, removeAllPolygons]);

  return null;
};
interface Center {
  lat: number;
  lng: number;
}
interface UpdateMapViewProps {
  center: Center;
  zoom: number;
}
const UpdateMapView = ({ center, zoom }:UpdateMapViewProps) => {
  const map = useMap();
  useEffect(() => {
    if (center && zoom) {
      map.setView([center.lat, center.lng], zoom);
    }
  }, [center, zoom, map]);
  return null;
};

function Map() {
  const [center, setCenter] = useState({ lat: 44.061936, lng: 20.658366 }); 
  const { areaStore, routStore, tourStore } = useStore();
  const { isCreatingArea, getAreas, selectedArea, isSelected, isLoadingAreas } = areaStore;
  const { isCreatingRout, setRoutLength, selectedRout } = routStore;
  const {showRout, tourRout} = tourStore;
  //const { totalPathDistance } = mapStore;

  console.log(showRout);
  const navigate = useNavigate();
  const [ZOOM_LEVEL, setZoom] = useState(7);

  const handleClick = (id: string) => {
    console.log('Kliknuli ste na poligon!', id);
    navigate(`/tours/${id}`);
  };

  // const _onDeleted = (e: any) => {
  //   console.log(e);
  //   deleteArea(selectedArea!.id);
  //   navigate('/');
  // };

  const _onCreated=(e: any)=>{
    const { layerType, layer } = e;
    let koordinate: Koordinata[] = [];
    let roadLength :number = 0;

    if (layerType === "polygon") {
      // Poligoni imaju složeniju strukturu
      koordinate = layer.getLatLngs()[0].map((coord: { lat: number; lng: number }) => ({
        latitude: coord.lat,
        longitude: coord.lng
      }));
      areaStore.setNewCoords(koordinate);
      
    } else if (layerType === "polyline") {
      // Polilinije vraćaju niz tačaka direktno
      const coords = layer.getLatLngs();

      koordinate = layer.getLatLngs().map((coord: { lat: number; lng: number }) => ({
        latitude: coord.lat,
        longitude: coord.lng
      }));
      for(let i=0; i<coords.length -1;i++)
        {
          roadLength += coords[i].distanceTo(coords[i + 1]);
        }
        setRoutLength(Number((roadLength/1000).toFixed(2)));
      console.log(`Ukupna dužina puta je ${roadLength} kilometara.`);

      routStore.setNewCoords(koordinate);
    }

};  

  // const _onEdited = (e: any) => {
  //   console.log(e);
  //   setLoadingInitial(true);
  //   const { layers: { _layers } } = e;
  //   agent.Areas.deleteAllCords(selectedArea!.id);

  //   // Pretpostavljamo da postoji samo jedan sloj na mapi
  //   const layerKey = Object.keys(_layers)[0];
  //   const { _leaflet_id, editing } = _layers[layerKey];
  //   const editedCoordinates = editing.latlngs[0];
  //   // Izvlačenje editovanih koordinata

  //   let koordinate: Koordinata[] = [];

  //   editedCoordinates.forEach((coord: any) => {
  //     coord.forEach((latlng: any) => {
  //       const novaKoordinata = new Koordinata();
  //       novaKoordinata.id = uuid();
  //       novaKoordinata.latitude = latlng.lat;
  //       novaKoordinata.longitude = latlng.lng;
  //       koordinate.push(novaKoordinata);
  //     });
  //   });
  //   console.log(`Edited coordinates for layer ${_leaflet_id}:`, koordinate);

  //   agent.Areas.addAllCoords(koordinate, selectedArea!.id);

    
  //   setLoadingInitial(false);

  // };
  useEffect(() => {
    if (showRout && tourRout && tourRout.koordinate.length > 0) {
      setCenter({ lat: tourRout.koordinate[0].latitude, lng: tourRout.koordinate[0].longitude });
      setZoom(13);
    }
    else if (showRout && selectedRout && selectedRout.koordinate.length > 0) {
      setCenter({ lat: selectedRout.koordinate[0].latitude, lng: selectedRout.koordinate[0].longitude });
      setZoom(13);
    } 
    else {

      setCenter({ lat: 44.061936, lng: 20.658366 });
      setZoom(7);
    }
  }, [showRout, tourRout]);
  

  return (
    <MapContainer center={ center} zoom={ZOOM_LEVEL} scrollWheelZoom={true}>
      <UpdateMapView center={center} zoom={ZOOM_LEVEL} />
      {isLoadingAreas ? (
        <LoadingComponent content='Ucitavanje podrucja...' />
      ) : (
        <>
          {(isCreatingArea || isCreatingRout) ? (
            <FeatureGroup>
              <EditControl
                position='topright'
                onCreated={_onCreated}
                draw={{
                  rectangle: false,
                  circle: false,
                  circlemarker: false,
                  polyline: isCreatingRout ? true : false,
                  marker: false,
                  polygon: isCreatingRout ? false : true
                }}
                edit={{
                  remove: false,
                  edit: false,
                }}
              />
            </FeatureGroup>
          ) : showRout ?
          ( tourRout ?
            <Polyline
                  key={tourRout!.id}
                  positions={
                    tourRout!.koordinate && tourRout!.koordinate.length > 0
                      ? tourRout!.koordinate.map((coord) => [
                          coord.latitude,
                          coord.longitude,
                        ] as LatLngExpression)
                      : []
                  }
                />
                :
                <Polyline
                  key={selectedRout!.id}
                  positions={
                    selectedRout!.koordinate && selectedRout!.koordinate.length > 0
                      ? selectedRout!.koordinate.map((coord) => [
                          coord.latitude,
                          coord.longitude,
                        ] as LatLngExpression)
                      : []
                  }
                />
          ) :
          
          isSelected ? (
            <Polygon
              key={selectedArea!.id}
              positions={
                selectedArea!.koordinate && selectedArea!.koordinate.length > 0
                  ? selectedArea!.koordinate.map((coord) => [
                    coord.latitude,
                    coord.longitude,
                  ] as LatLngExpression)
                  : []
              }
            />
          ) : (
            getAreas.map((layer) => (
              <Polygon
                key={layer.id}
                positions={
                  layer.koordinate && layer.koordinate.length > 0
                    ? layer.koordinate.map((coord) => [
                      coord.latitude,
                      coord.longitude,
                    ] as LatLngExpression)
                    : []
                }
                eventHandlers={{ click: () => handleClick(layer.id) }}
              />
            ))
          )}
        </>
      )}
      <TileLayer attribution={osm.maptiler.attribution} url={osm.maptiler.url} />
    </MapContainer>
  );
}

export default observer(Map);
