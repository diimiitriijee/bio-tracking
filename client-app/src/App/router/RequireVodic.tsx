import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useStore } from "../stores/store";

export default function RequireVodic (){
    const {userStore : {isLoggedIn, isAdmin, isVodic}} = useStore();
    const location = useLocation();

    if(!isLoggedIn) {
        return <Navigate to='/login' state={{from: location}} />
    }
    if(!isVodic && !isAdmin){
        return <Navigate to='/not-found' state={{from: location}} />
    }

    return <Outlet />
}