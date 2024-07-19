import { makeAutoObservable, runInAction } from "mobx";
import { RegisterUserFormValues, User, UserFormValues } from "../modules/User";
import agent from "../api/agent";
import { store } from "./store";
import { router } from "../router/Routes";

export default class UserStore {
    user: User | null = null;
    refreshTokenTimeout?: number;

    constructor(){
        makeAutoObservable(this);
    }

    get isLoggedIn(){
        return !!this.user;
    }

    get isAdmin() {
        if (!this.user) return false;
        const token = this.getDecodedToken(this.user.token);
        return token["role"] && token["role"].includes("Administrator");
    }

    get isVodic() {
        if (!this.user) return false;
        const token = this.getDecodedToken(this.user.token);
        return token["role"] && token["role"].includes("TuristickiVodic");
    }

    get isUser() {
        if (!this.user) return false;
        const token = this.getDecodedToken(this.user.token);
        return token["role"] && token["role"].includes("ObicanKorisnik");
    }

    login = async (creds: UserFormValues) => {
        try{
            const user = await agent.Account.login(creds);
            store.commonStore.setToken(user.token);
            this.startRefreshTokenTimer(user);//ovo treba da pozovemo svuda kad dobijemo usera od server
            runInAction(()=> this.user = user);
            router.navigate('/');
            store.modalStore.closeModal();
        } catch(error){
            throw error;
        }
        
    }

    // register = async (creds: UserFormValues) => {
    //     try{
    //         const user = await agent.Account.register(creds);
    //         store.commonStore.setToken(user.token);
    //         this.startRefreshTokenTimer(user);//ovo treba da pozovemo svuda kad dobijemo usera od server
    //         runInAction(()=> this.user = user);
    //         router.navigate('/');
    //         store.modalStore.closeModal();
    //     } catch(error){
    //         throw error;
    //     }
        
    // }
    register = async (podaci: RegisterUserFormValues) => {
        try{
            await agent.Account.register(podaci);
            router.navigate(`/Account/registerSuccess?email=${podaci.email}`);
            store.modalStore.closeModal();
        } catch(error){
            throw error;
        }
        
    }
    // registerVodic = async (podaci: FormData) => {
    //     try {
    //         console.log(podaci);
    //         const user = await agent.Account.registerVodic(podaci);
    //         store.commonStore.setToken(user.token);
    //         this.startRefreshTokenTimer(user);//ovo treba da pozovemo svuda kad dobijemo usera od server
    //         runInAction(() => this.user = user);
    //         router.navigate('/');
    //         store.modalStore.closeModal();
    //     } catch (error) {
    //         throw error;
    //     }
    // };
    registerVodic = async (podaci: FormData) => {
        try {
            console.log(podaci);
            await agent.Account.registerVodic(podaci);
            router.navigate(`/Account/registerSuccessGuide?email=${podaci.get('email')}`);
            store.modalStore.closeModal();
        } catch (error) {
            throw error;
        }
    };
    
    

    loguot = () =>{
        store.commonStore.setToken(null);
        this.user = null;
        router.navigate('/');
    }

    getUser = async () =>{
        try{
            const user = await agent.Account.current();
            store.commonStore.setToken(user.token);//uvek kad dobijemo usera sa servera setujemo token i startujemo timer
            this.startRefreshTokenTimer(user);//ovo treba da pozovemo svuda kad dobijemo usera od server
            runInAction(()=> this.user = user);
        } catch(error){
            console.log(error);
        }
    }

    setImage = (image:string) =>{
        if (this.user) this.user.slika = image;
    }

    getDecodedToken(token : string){
        return JSON.parse(atob(token.split('.')[1]))// delimo token po . jer je prvi deo algoritam, pa drugi deo iza tacke nama bitne informacije, pa treci deo iza tacke potpis sto nas ne zanima, pa sa [1] uzimamo samo informacije koje nas zanimaju
    }//

    refreshToken = async () => {
        this.stopRefreshTokenTimer();
        try
        {
            const user = await agent.Account.refreshToken();
            runInAction(() => this.user = user);
            store.commonStore.setToken(user.token);
            this.startRefreshTokenTimer(user);//ovo treba da pozovemo svuda kad dobijemo usera od server
        }
        catch(error)
        {
            console.log(error);
        }
    }

    private startRefreshTokenTimer(user: User){
        const jwToken = JSON.parse(atob(user.token.split('.')[1]));//atob vadi info iz tokena
        const expires = new Date(jwToken.exp * 1000);
        const timeout = expires.getTime() - Date.now() - (30 * 1000);//tajmaut od 30 sekunde
        this.refreshTokenTimeout = setTimeout(this.refreshToken, timeout);//kad istekne token pozivam ovu metodu iznad da nam server da novi token
        console.log({refreshTimeout: this.refreshTokenTimeout});
    }

    private stopRefreshTokenTimer() {
        clearTimeout(this.refreshTokenTimeout)
    }

    changePassword = async (oldPassword: string, newPassword: string, confirmedNewPassword: string ) => {
        try {
            console.log(oldPassword);//pa ne treba da baca anauthorized za gresku u sifru
            await agent.Account.changePassword(oldPassword, newPassword, confirmedNewPassword);
        } catch (error) {
            throw error;
        }
    }
}