import { PrijavljeniObilazak } from "./PrijavljeniObilazak"

export type Korisnik = {
    id: string,
    ime: string,
    prezime: string,
    telefon: string,
    datumRodjenja: string,
    email: string,
    userName: string,
    password: string,
    slikaProfila: string,
    prijavljeniObilasci: PrijavljeniObilazak[]
  }



