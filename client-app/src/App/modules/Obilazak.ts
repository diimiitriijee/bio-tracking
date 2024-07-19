import { Komentar } from "./Komentari"
import { Profil } from "./Profil"
import { Ruta } from "./Ruta";
import { Vodic } from "./Vodic";

export interface Obilazak {
    id: string;
    naziv: string;
    opis: string;
    datumOdrzavanja: Date | null;
    brojMaxPolaznika: number | null;
    mestoOkupljanja: string;
    komentari?: Komentar[];
    alergije?: any[];
    isCancelled?: boolean;
    vodic: Vodic;
    ide:boolean;
    isVodic:boolean;
    ucesnici: Profil[];
    ruta: Ruta;
}

export class Obilazak implements Obilazak{

    constructor(init?:ObilazakFromValues){
        Object.assign(this,init);
    }
}

export class ObilazakFromValues{
    id?: string = undefined;
    naziv: string = '';
    opis: string = '';
    datumOdrzavanja: Date | null = null;
    brojMaxPolaznika: number | null = null;
    mestoOkupljanja: string = '';
    isCancelled: boolean = false;

    constructor(tour?: ObilazakFromValues){
        if(tour){
            this.id = tour.id;
            this.naziv = tour.naziv;
            this.opis = tour.opis;
            this.datumOdrzavanja = tour.datumOdrzavanja;
            this.brojMaxPolaznika = tour.brojMaxPolaznika;
            this.mestoOkupljanja = tour.mestoOkupljanja;
        }
    }
}