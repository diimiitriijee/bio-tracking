import { Navigate, RouteObject, createBrowserRouter } from "react-router-dom";
import App from "../layout/App";
import HomePage from "../features/HomePage/HomePage";
import TourForm from "../features/Tours/form/TourForm";
import Tours from "../features/Tours/Tours";
import TourDetails from "../features/Tours/details/TourDetails";
import TestErrors from "../features/Errors/TestError";
import NotFound from "../features/Errors/NotFound";
import ServerError from "../features/Errors/ServerError";
import LoginForm from "../features/users/LoginForm";
import Areas from "../features/Area/Areas";
import PlantForm from "../features/Plants/form/PlantForm";
import PlantDetails from "../features/Plants/Details/PlantDetails";
import ProfilePage from "../features/Profiles/ProfilePage";
import AdminDashBoard from "../features/Admin/Dashboard/AdminDashBoard";
import AreaForm from "../features/Area/Form/AreaForm";
import RoutForm from "../features/Rout/Form/RoutForm";
import AreaRoutes from "../features/Rout/AreaRoutes";
import RegisterSuccess from "../features/users/RegisterSuccess";
import ConfirmEmail from "../features/users/ConfirmEmail";
import RegisterSuccessGuide from "../features/users/RegisterSuccessGuide";
import RoutDetails from "../features/Rout/details/RoutDetails";
import RequireAuth from "./RequireAuth";
import RequireAdmin from "./RequireAdmin";
import RequireVodic from "./RequireVodic";
import GuideDashBoard from "../features/Admin/GuideRequests/GuideDashBoard";

export const routes: RouteObject[] = [
    {
        path:"/",
        element: <App/>,
        children:[
            {
                element: <RequireAuth />, children: [
                    {path: 'profiles/:username', element:<ProfilePage />}
                ]
            },
            {
                element: <RequireAdmin />, children: [
                    {path: 'adminDashboard', element: <AdminDashBoard /> },
                    {path: 'errors', element:<TestErrors/>},
                    {path: 'guideRequests', element: <GuideDashBoard />}
                ]
            },
            {
                element: <RequireVodic />, children: [//oovo sve moze i admin tako sam namestio u RequireVodic ako ocemo da ga iskljucujemo
                    {path: 'manage/:id', element:<TourForm key='manage'/>},
                    {path: 'createTour', element:<TourForm key='create'/>},
                    {path: 'createPlant', element:<PlantForm key='create' />},
                    {path: 'managePlant/:id', element:<PlantForm key='manage' />},
                    {path: 'createArea', element:<AreaForm />},
                    {path: 'createRout', element:<RoutForm />},
                ]
            },//za ovo sve ostalo moze i anonimni korisnik
            {path:'', element:<HomePage />},
            {path: 'allTours', element: <Tours />},
            {path: 'tours/:id', element:<Tours/>},
            {path: 'tour/:id', element:<TourDetails/>},
            {path: 'areaDetails/:id', element:<Areas />},
            {path: 'login', element:<LoginForm/>},
            {path: 'not-found', element:<NotFound/>},
            {path: 'server-error', element:<ServerError/>},
            {path: 'account/registerSuccess', element:<RegisterSuccess/>},
            {path: 'account/registerSuccessGuide', element:<RegisterSuccessGuide />},
            {path: 'account/verifyEmail', element:<ConfirmEmail/>},
            {path: '*', element:<Navigate replace to='/not-found'/>},
            {path: 'plantDetails/:id', element:<PlantDetails />},
            {path: 'areaRouts/:id', element:<AreaRoutes />},
            {path: 'routDetails/:id', element:<RoutDetails />},

        ]
    }
    
]

export const router = createBrowserRouter(routes);  