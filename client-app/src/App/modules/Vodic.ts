export interface Vodic{
    
  strucnaSprema: string;
  brojOdrzanihObilazaka: number;
  ocene?: any[];
  username: string;
  ime: string;
  prezime: string;
  telefon?: any;
  datumRodjenja: string;
  slikaProfila?: any;
  brojPrijavljenihObilazaka?: number;
  prosecnaOcena?:number;
  followersCount?:number;

}

export class Vodic implements Vodic{
    constructor(init?:VodicFromValues){
        Object.assign(this,init);
}}

export class VodicFromValues{

  strucnaSprema: string = '';
  brojOdrzanihObilazaka: number = 0;
  ocene?: any[] = undefined;
  username: string = '';
  ime: string = '';
  prezime: string = '';
  telefon?: any  = undefined;
  datumRodjenja: string = '';
  slikaProfila?: any = undefined;
  followersCount?:number = 0;

  constructor(vodic?:VodicFromValues){
    if(vodic){
        this.strucnaSprema = vodic.strucnaSprema;
        this.brojOdrzanihObilazaka = vodic.brojOdrzanihObilazaka;
        this.ocene = vodic.ocene;
        this.username = vodic.username;
        this.ime = vodic.ime;
        this.prezime = vodic.prezime;
        this.telefon = vodic.telefon;
        this.datumRodjenja = vodic.datumRodjenja;
        this.slikaProfila = vodic.slikaProfila;
    }
  }


}

export interface VodicZahtev{
  id: string;
  ime: string;
  prezime: string;
  email: string;
  username: string;
  telefon: string;
  datumRodjenja: string;
  strucnaSprema: string;
  brojOdrzanihObilazaka: number;
  putanjaSlikeDiplome: string;
}