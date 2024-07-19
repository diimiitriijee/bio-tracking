import { makeAutoObservable, runInAction } from "mobx";
import agent from "../api/agent";
import { User, UserForAdmin } from "../modules/User";
import { VodicZahtev } from "../modules/Vodic";

export default class AdminStore {
    userRegistry = new Map<string, UserForAdmin>();
    selectedUser: User | undefined = undefined;
    loading = false;
    loadingInitial = false;
    guideRegistry = new Map<string,VodicZahtev>();

    constructor() {
        makeAutoObservable(this);
    }

    get users() {
        return Array.from(this.userRegistry.values());
    }

    get guides() {
        return Array.from(this.guideRegistry.values());
    }

    loadUsers = async () => {
        this.loadingInitial = true;
        try {
            const users = await agent.Admin.list();
            runInAction(() => {
                users.forEach(user => {
                    this.userRegistry.set(user.userName, user);
                });
                this.loadingInitial = false;
            });
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loadingInitial = false;
            });
        }
    }

    loadGuideRequests = async () => {
        this.loadingInitial = true;
        try {
            const guides = await agent.Admin.listRequestedGuides();
            runInAction(() => {
                guides.forEach(guide => {
                    this.guideRegistry.set(guide.id, guide);
                });
                this.loadingInitial = false;
            });
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loadingInitial = false;
            });
        }
    }

    clearUsers = () => {
        this.userRegistry.clear();
    }

    banUser = async (userId: string) => {
        this.loading = true;
        try {
            await agent.Admin.ban(userId);
            runInAction(() => {
                this.loading = false;
            });
        } catch (error) {
            console.error(error);
            runInAction(() => {
                this.loading = false;
            });
        }
    }

    postGuide = async (id:string) =>{
        this.loading = true;
        try{
            await agent.Admin.postGuide(id);
            runInAction(()=>{
                this.guideRegistry.delete(id);
                this.loading = false;
            })
        } catch (error){
            console.log(error);
            runInAction(() => {
                this.loadingInitial = false;
            });
        }
    }
}
