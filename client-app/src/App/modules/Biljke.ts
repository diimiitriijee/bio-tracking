export interface Biljka {
    id: string,
    naziv: string,
    opis: string,
    vrsta: string,
    lekovita: boolean,
    slika: string
  }

export class Biljka implements Biljka{
  constructor(init?:BiljkaFormValues){
    Object.assign(this,init);
  }
}

export class BiljkaFormValues{  
  id?: string = undefined;
    naziv: string = '';
    opis: string = '';
    vrsta: string = '';
    lekovita: boolean = false;
    slika: string = '';


    constructor(plant?:BiljkaFormValues){
      if(plant){
        this.id = plant.id;
        this.naziv = plant.naziv;
        this.opis = plant.opis;
        this.vrsta = plant.vrsta;
        this.lekovita = plant.lekovita;
        this.slika = plant.slika;
      }
    }
}
  