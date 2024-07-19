import { makeAutoObservable, runInAction } from "mobx";
import { ObilazakKorisnika, Photo, Profil } from "../modules/Profil";
import agent from "../api/agent";
import { store } from "./store";

export default class ProfileStore{
    profile:Profil | null = null;
    loadingProfile = false;
    uploading = false;
    loading = false;
    loadingTours = false;
    userTours: ObilazakKorisnika[] = [];

    constructor(){
        makeAutoObservable(this);
    }

    get isCurrentUser(){
        if(store.userStore.user && this.profile)
            return store.userStore.user.userName === this.profile.username;

        return false;
    }

    loadProfile = async(username:string) =>{
        this.loadingProfile = true;
        try{
            const profile = await agent.Profiles.get(username);
            console.log("Ucitan profil " + profile.username)
            runInAction(()=>{
                this.profile=profile;
            })
        }catch(error)
        {
            console.log(error);
        } finally{
            this.loadingProfile = false;
        }
    }

    loadVodicProfile = async(username:string) =>{
        this.loadingProfile = true;
        try{
            const profile = await agent.Profiles.getVodic(username);
            console.log("Ucitan profil vodica "+profile.username)
            runInAction(()=>{
                this.profile=profile;
            })
        }catch(error)
        {
            console.log(error);
        } finally{
            this.loadingProfile = false;
        }
    }

    uploadPhoto = async (file:Blob) =>{
        this.uploading = true;
        try{
            const response = await agent.Profiles.uploadPhoto(file);
            const photo = response.data;
            runInAction(()=>{
                if(this.profile){
                    this.profile.slike?.push(photo);
                    if(photo.isMain && store.userStore.user){
                        store.userStore.setImage(photo.url);
                        this.profile.slikaProfila = photo.url;
                    }
                }
            })
        }catch(error){
            console.log(error);
            runInAction(() => {
                this.uploading = false;
            });
        } finally{
            runInAction(() => {
                this.uploading = false;
            });
        }

    }

    setMainPhoto = async (photo:Photo) =>{
        this.loading= true;
        try{
            await agent.Profiles.setMainPhoto(photo.id);
            store.userStore.setImage(photo.url);
            runInAction(()=>{
                if(this.profile && this.profile.slike){
                    this.profile.slike.find(p => p.isMain)!.isMain = false;
                    this.profile.slike.find(p => p.id === photo.id)!.isMain = true;
                    this.profile.slikaProfila = photo.url;
                    this.loading = false;
                }
            })
        } catch(error){
            console.log(error);
            runInAction(()=>{
                this.loading= false;
            })
        } 
    }

    deletePhoto = async (photo:Photo)=>{
        this.loading = true;
        try{
            await agent.Profiles.deletePhoto(photo.id);
            runInAction(()=>{
                if(this.profile){
                    this.profile.slike = this.profile.slike?.filter(p => p.id !== photo.id);
                }
            })

        } catch(error){
            console.log(error);
        } finally{
            this.loading = false;
        }
    }

    loadUserTours = async (username: string, predicate?:string) => {
        this.loadingTours = true;
        try{
            const tours = await agent.Profiles.listTours(username, predicate!);
            runInAction(()=>{
                this.userTours = tours;
                this.loadingTours = false;
            }) 
        } catch (error){
            console.log(error);
            runInAction(()=>{
                this.loadingTours = false;
            })
        }
    }

    updateFollowing = async (username: string, following: boolean) =>{
        this.loading = true;
        try{
            await agent.Follow.updateFollowing(username);
            runInAction(()=>{
                if(this.profile && this.profile.username !== store.userStore.user?.userName){
                    following ? this.profile.followersCount++ : this.profile.followersCount--;
                    this.profile.following = !this.profile.following;
                }
                this.loading = false;
            })
        } catch (error){
            console.log(error);
            runInAction(()=>this.loading = false)
        }
    }
}