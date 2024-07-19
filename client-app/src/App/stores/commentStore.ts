import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { makeAutoObservable, runInAction } from "mobx";
import { store } from "./store";
import { Komentar } from "../modules/comment";

export default class CommentStore {
    comments: Komentar[] = [];
    hubConnection: HubConnection | null = null;//ovo dobijamo iz signalr

    constructor() {
        makeAutoObservable(this);
    }

    createHubConnection = (obilazakId: string) => {//prosledjuje se odma ovaj obilazakId da bi mogo da se konektuje ovde pre nego da se aktivnost ucita do kraja pa se tako smanjuje latencija
        if (store.tourStore.selectedTour) {//prvo proverim da l imamo selektovani obilazak
            console.log('do ovde radi')
            this.hubConnection = new HubConnectionBuilder()
                .withUrl("http://localhost:5000/chat" + '?obilazakId=' + obilazakId, {
                    accessTokenFactory: () => store.userStore.user?.token as string
                })
                .withAutomaticReconnect()//ako korisnik izgubi konekciju da odma probam da ga rekonektujem
                .configureLogging(LogLevel.Information)
                .build();

            this.hubConnection.start().catch(error => console.log('Error establishing connection: ', error));//startuje konekciju

            this.hubConnection.on('LoadComments', (comments: Komentar[]) => { //da dobijamo listu svih komentara
                runInAction(() => {
                    comments.forEach(comment => {
                        comment.datumKreiranja = new Date(comment.datumKreiranja);
                    });
                    this.comments = comments;
                });
            });

            this.hubConnection.on('ReceiveComment', (comment : Komentar)=> {//dobije komentar sa servera i ubaci ga u nasu listu komentara, to radi i za nas komentar koji posaljemo 
                runInAction(() => {
                    comment.datumKreiranja = new Date(comment.datumKreiranja);
                    this.comments.unshift(comment);//da bi ga ubacio sa unshift na pocetak a ne da padaju uvek nadole
                })
            })
        }
    }

    stopHubConnection = () => {
        this.hubConnection?.stop().catch(error => console.log('Error stopping connection: ', error));
    }

    clearComments = () => {//kad korisnik izadje iz taj obilazak koji je gledao da se obrisu komentariti i da se zaustavi konekcija
        this.comments = [];
        this.stopHubConnection();
    }

    addComment = async (values: {tekst: string, obilazakId?: string}) => {
        values.obilazakId = store.tourStore.selectedTour?.id;
        try {
            await this.hubConnection?.invoke('SendComment', values);
        } catch (error) {
            console.log(error);
        }
    }
}