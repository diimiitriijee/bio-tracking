import { createContext, useContext } from "react";
import TourStore from "./tourStore";
import CommonStore from "./commonStore";
import UserStore from "./userStore";
import ModalStore from "./modalStore";
import PlantStore from "./plantStore";
import AreaStore from "./areaStore";
import ProfileStore from "./profileStore";
import CommentStore from "./commentStore";
import AdminStore from "./adminStore";
import RoutStore from "./routStore";
import MapStore from "./mapStore";

interface Store{
    tourStore:TourStore;
    commonStore: CommonStore;
    userStore : UserStore;
    modalStore: ModalStore;
    plantStore: PlantStore;
    areaStore: AreaStore;
    profileStore: ProfileStore;
    commentStore: CommentStore;
    adminStore: AdminStore;
    routStore: RoutStore;
    mapStore: MapStore;
}

export const store: Store ={
    tourStore:new TourStore(),
    commonStore: new CommonStore(),
    userStore : new UserStore(),
    modalStore: new ModalStore(),
    plantStore: new PlantStore(),
    areaStore: new AreaStore(),
    profileStore: new ProfileStore(),
    commentStore: new CommentStore(),
    adminStore: new AdminStore(),
    routStore: new RoutStore(),
    mapStore: new MapStore()
}

export const StoreContext = createContext(store);

export function useStore(){
    return useContext(StoreContext);
}