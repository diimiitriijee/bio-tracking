import { Biljka } from "./Biljke";
import { Koordinata } from "./Koordinata";

export interface Podrucje{
    id: string;
    oblast: string;
    koordinate: Koordinata[];
    biljke: Biljka[];
}

export class Podrucje implements Podrucje{
    constructor(init?:PodrucjeFormValue){
        Object.assign(this,init);
    }
}

export class PodrucjeFormValue{
    id: string = '';
    oblast: string = '';
    koordinate: Koordinata[] = [];
    biljke: Biljka[] = [];

    constructor(podrucje?:PodrucjeFormValue){
        if(podrucje){
            this.id = podrucje.id;
            this.oblast = podrucje.oblast;
            this.koordinate = podrucje.koordinate;
            this.biljke = podrucje.biljke;
        }
    }
}