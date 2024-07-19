export interface Koordinata{
    id: string;
    latitude: number;
    longitude: number;
}

export class Koordinata implements Koordinata{
    constructor(init?:KoordinaFormValue){
        Object.assign(this,init);
    }
}

export class KoordinaFormValue{
    id: string = '';
    latitude: number = 0;
    longitude: number = 0;

    constructor(koordinata:KoordinaFormValue){
        this.id = koordinata.id;
        this.latitude = koordinata.latitude;
        this.longitude = koordinata.longitude;
    }
}