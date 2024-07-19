import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useStore } from "../stores/store";

export default function RequireAdmin (){
    const {userStore : {isLoggedIn, isAdmin}} = useStore();
    const location = useLocation();

    if(!isLoggedIn) {
        return <Navigate to='/login' state={{from: location}} />
    }
    if(!isAdmin){
        return <Navigate to='/not-found' state={{from: location}} />
    }

    return <Outlet />
}