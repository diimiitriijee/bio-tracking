import { Photo } from "./Profil";

export interface User{
    ime: string;
    prezime:string;
    token:string;
    //slika?:Photo;
    slika?:string;
    userName:string;
    roles : Role[];
    brojPrijavljenihObilazaka: number;
}

export interface UserFormValues {
    email:string;
    password: string;
    ime?: string;
    prezime?: string;
    username?: string;
}

export interface RegisterUserFormValues {
  email:string;
  password: string;
  ime?: string;
  prezime?: string;
  username?: string;
  telefon: string;
  datumRodjenja: Date;
}

export interface RegisterVodicFormValues {
  email:string;
  ime?: string;
  prezime?: string;
  username?: string;
  telefon: string;
  datumRodjenja: Date;
  strucnaSprema: string;
  slikaDiplome: any;
}

export interface Role {
    korisnikRoles: any;
    id: string;
    name: string;
    normalizedName: string;
    concurrencyStamp: any;
  }

  export interface UserForAdmin{
    id: string;
    ime: string;
    prezime: string;
    userName: string;
    slika?: Photo;
    roles: Role[]
  }