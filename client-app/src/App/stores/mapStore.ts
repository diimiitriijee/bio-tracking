import { makeAutoObservable } from "mobx";
import { Koordinata } from "../modules/Koordinata";

export default class MapStore{

    constructor(){
        makeAutoObservable(this);
    }

    

     haversineDistance(coord1:Koordinata, coord2:Koordinata) {
    const toRadians = (degree: number) => degree * Math.PI / 180;

    const R = 6371; // Poluprečnik Zemlje u kilometrima

    const dLat = toRadians(coord2.latitude - coord1.latitude);
    const dLon = toRadians(coord2.longitude - coord1.longitude);

    const a =
        Math.sin(dLat / 2) * Math.sin(dLat / 2) +
        Math.cos(toRadians(coord1.latitude)) * Math.cos(toRadians(coord2.latitude)) *
        Math.sin(dLon / 2) * Math.sin(dLon / 2);

    const c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));

    const distance = R * c;
    return distance; // Udaljenost u kilometrima
}
    // Funkcija za računanje ukupne dužine puta
    totalPathDistance = (koordinate:Koordinata[]) => {
    let totalDistance = 0;

    for (let i = 0; i < koordinate.length - 1; i++) {
        totalDistance += this.haversineDistance(koordinate[i], koordinate[i + 1]);
    }

    return totalDistance; // Ukupna dužina puta u kilometrima
}
}