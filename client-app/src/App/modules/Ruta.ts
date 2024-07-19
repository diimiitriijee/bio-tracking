import { Koordinata } from "./Koordinata"

export interface Ruta{
    id: string;
    opis: string;
    duzina: number;
    tip: string;
    zaDecu: boolean;
    prohodnost: string;
    uspon: string;
    koordinate: Koordinata[];
}

export class Ruta implements Ruta{
    constructor(init?:RutaFormValues){
        Object.assign(this,init);
    }
}

export class RutaFormValues{
    id?: string = undefined;
    opis: string ='';
    duzina?: number = undefined;
    tip: string = '';
    zaDecu: boolean = false;
    prohodnost: string = '';
    uspon: string = '';

    constructor(rout?:RutaFormValues){
        if(rout)
        {
            this.id = rout.id;
            this.opis = rout.opis;
            this.duzina = rout.duzina;
            this.tip = rout.tip;
            this.zaDecu = rout.zaDecu;
            this.prohodnost = rout.prohodnost;
            this.uspon = rout.uspon;
        }
        
        
    }
}