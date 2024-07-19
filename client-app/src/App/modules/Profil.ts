import { User } from "./User"

export interface Profil{
    username: string;
    ime: string;
    prezime: string;
    telefon?: any;
    datumRodjenja?: Date | null;
    //slikaProfila?: Photo;
    slikaProfila?: string;
    slike?: Photo[];
    brojOdrzanihObilazaka?:number;
    brojPrijavljenihObilazaka?:number;
    prosecnaOcena?:number;
    followersCount:number;
    following:boolean;
}

export class Profil implements Profil{
    username: string;
    ime: string;
    prezime: string;
    telefon?: any;
    datumRodjenja?: Date | null;
    //slikaProfila?: Photo;
    slikaProfila?: string;
    slike?: Photo[];
    brojOdrzanihObilazaka?:number;
    brojPrijavljenihObilazaka?:number;
    prosecnaOcena?:number;

    constructor(user:User){
        this.username = user.userName;
        this.ime = user.ime;
        this.prezime = user.prezime;
        this.slikaProfila = user.slika;
        this.brojPrijavljenihObilazaka = user.brojPrijavljenihObilazaka;
    }
}

export interface Photo{
    id:string;
    url:string;
    isMain:boolean;
}

export interface ObilazakKorisnika{
    id: string
    naziv: string
    opis: string
    datumOdrzavanja: string
}
