import { Obilazak } from "./Obilazak"

export type PrijavljeniObilazak = {
    id: string
    obilazak: Obilazak
    korisnik: string
    datumPrijave: string
}