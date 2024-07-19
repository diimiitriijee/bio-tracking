import {Korisnik} from "./Korisnik"

export type Komentar = {
    id: string
    tekst: string
    datumKreiranja: string
    korisnik: Korisnik
    obilazak: string
}