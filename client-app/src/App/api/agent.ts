import axios, { AxiosError, AxiosResponse } from 'axios';
import { Obilazak, ObilazakFromValues } from '../modules/Obilazak';
import { toast } from 'react-toastify';
import { router } from '../router/Routes';
import { store } from '../stores/store';
import { RegisterUserFormValues, /*RegisterVodicFormValues, */User, UserForAdmin, UserFormValues } from '../modules/User';
import { Biljka, BiljkaFormValues } from '../modules/Biljke';
import { Podrucje, PodrucjeFormValue } from '../modules/Podrucje';
import { ObilazakKorisnika, Photo, Profil } from '../modules/Profil';
import { Koordinata } from '../modules/Koordinata';
import { PaginatedResult } from '../modules/Pagination';
import { Ruta, RutaFormValues } from '../modules/Ruta';
import { VodicZahtev } from '../modules/Vodic';

const sleep =(delay: number) =>{
    return new Promise((resolve)=>{
        setTimeout(resolve,delay)
    })
}

//axios.defaults.baseURL='http://localhost:5000/api';
axios.defaults.baseURL=import.meta.env.VITE_API_URL;

axios.interceptors.request.use(config => {
    const token= store.commonStore.token;
    if(token && config.headers) config.headers.Authorization = `Bearer ${token}`;
    return config;
})

axios.interceptors.response.use(async response =>{
        if (import.meta.env.DEV) await sleep(1000);// NOVO
        const pagination = response.headers['pagination'];
        if(pagination){
            response.data = new PaginatedResult(response.data,JSON.parse(pagination));
            return response as AxiosResponse<PaginatedResult<any>>
        }
        return response;

}, (error: AxiosError) =>{
    const {data,status, config, headers} = error.response as AxiosResponse;
    switch (status) {
        case 400:
            if(config.method === 'get' && Object.prototype.hasOwnProperty.call(data.errors,'id')){
                router.navigate('/not-found');
            }
            if(data.errors){
                const modalStateErrors =[];
                for(const key in data.errors){
                    if(data.errors[key]){
                        modalStateErrors.push(data.errors[key])
                    }
                }
                throw modalStateErrors.flat();
            } else{
                toast.error(data);
                //console.log("odavde");
            }
            toast.error('bad request');
            //console.log("odavde");
            break;

        case 401:
            if(status === 401 && headers['www-authenticate']?.startsWith('Bearer error="invalid_token'))
            {
                store.userStore.loguot();
                toast.error('Sesija je istekla - molimo Vas da se ulogujete ponovo.');
            } else{
                toast.error('unauthorised');
            }
            break;
            
        case 403:
            toast.error('forbidden');
            break;
            
        case 404:
            router.navigate('/not found');
            break;
            
        case 500:
            store.commonStore.setServerError(data);
            router.navigate('/server-error');
            break;
    }
    return Promise.reject(error);
})

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url:string) => axios.get<T>(url).then(responseBody),
    post: <T>(url:string, body:{}) => axios.post<T>(url, body).then(responseBody),
    postFormData: <T>(url: string, formData: FormData) => axios.post<T>(url, formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
    }).then(responseBody),
    put: <T>(url:string,body:{}) => axios.put<T>(url,body).then(responseBody),
    del: <T>(url:string)=> axios.delete<T>(url).then(responseBody)
}

const Tours = {
    list: (id:string,params:URLSearchParams)=> axios.get<PaginatedResult<Obilazak[]>>(`/Obilazak/VratiObilaskePodrucja/${id}`,{params})
    .then(responseBody),
    listAllTours: (params:URLSearchParams) => axios.get<PaginatedResult<Obilazak[]>>('/Obilazak/VratiSveObilaske', {params})
        .then(responseBody),
    details: (id:string) => requests.get<Obilazak>(`/Obilazak/VratiObilazak/${id}`),
    create: (tour:ObilazakFromValues,id:string) => requests.post<void>(`/Obilazak/KreirajObilazak/${id}`,tour),
    update: (tour: ObilazakFromValues) => requests.put<void>(`/Obilazak/IzmeniObilazak/${tour.id}`,tour),
    delete: (id:string) => requests.del<void>(`/Obilazak/ObrisiObilazak/${id}`),
    attend: (id:string) => requests.post<void>(`/Obilazak/DodajUcesnika/${id}`,{}),
    cancleAttend:(id:string) => requests.del<void>(`/Obilazak/UkloniUcesnika/${id}`),
    rateGuide: (id: string, vrednostOcene: number, komentar: string) => axios.post<void>(`/Obilazak/OceniVodica/${id}`, {vrednostOcene, komentar}),
    createTourWithRout: (idArea:string, idRout:string, tour:ObilazakFromValues) => requests.post<void>(`/Obilazak/KreirajObilazak/${idArea}/${idRout}`, tour)

}

const Plants = {
    list: () => requests.get<Biljka[]>('/Biljka/VratiBiljke'),
    curentPlant: (id:string) => requests.get<Biljka>(`/Biljka/VratiBiljku/${id}`),
    create: (plant:FormData) => requests.postFormData<void>(`/Biljka/KreirajBiljku`, plant),
    update: (plant:BiljkaFormValues) => requests.put<void>(`/Biljka/IzmeniBiljku/${plant.id}`,plant),
    delete: (id:string) => requests.del<void>(`/Biljka/ObrisiBiljku/${id}`),
    listAreas: (id:string) => requests.get<PodrucjeFormValue[]>(`/Biljka/VratiPodrucjaBiljke/${id}`)
        
}

const Areas = {
    current: (id:string) => requests.get<Podrucje>(`/Podrucje/VratiPodrucje${id}`),
    list: () => requests.get<Podrucje[]>('/Podrucje/VratiSvaPodrucja'),
    plantList: (id:string,params:URLSearchParams) => axios.get<PaginatedResult<Biljka[]>>(`/Podrucje/VratiBiljkePodrucja/${id}`,{params})
        .then(responseBody),
    create: (podrucje:PodrucjeFormValue) => requests.post<void>('/Podrucje/KreirajPodrucje', podrucje),
    update: (podrucje:PodrucjeFormValue) => requests.put<void>(`/Podrucje/IzmeniPodrucje${podrucje.id}`, podrucje),
    delete: (id:string) => requests.del<void>(`/Podrucje/ObrisiPodrucje/${id}`),
    addPlant: (idArea:string,plant:Biljka) => requests.post<void>(`/Podrucje/DodajBiljku/${idArea}/${plant.id}`,plant),
    deletePlant: (idArea:string,idPlant:string) => requests.del<void>(`/Podrucje/IzbaciBiljku/${idArea}/${idPlant}`),
    updateCoords: (coords:Koordinata[],idArea:string) => requests.put<void>(`/Podrucje/IzmeniKoordinatePodrucja/${idArea}`,coords),
    deleteAllCords: (id:string) => requests.del<void>(`/Podrucje/ObrisiKoorde/${id}`),
    addAllCoords: (coords:Koordinata[],idArea:string) => requests.post<void>(`/Podrucje/DodajKoorde/${idArea}`, coords)
}
const Account ={
    current: () => requests.get<User>('/Account'),
    login: (user:UserFormValues) => requests.post<User>('/Account/login', user),
    register: (user: RegisterUserFormValues) => requests.post<User>('/Account/register', user),
    registerVodic: (vodic: FormData) => requests.postFormData<User>('/Account/register-vodic', vodic),
    refreshToken: () => requests.post<User>('/Account/refreshToken/', {}),
    verifyEmail: (token: string, email : string) => requests.post<void>(`/Account/verifyEmail?token=${token}&email=${email}`, {}),
    resendEmailConfirmed: (email : string) => requests.get(`/Account/resendEmailConfirmation?email=${email}`),
    changePassword: (oldPassword: string, newPassword: string, confirmPassword: string ) => requests.post<void>('/Account/change-password', {oldPassword, newPassword, confirmPassword})
};

const Profiles ={
    get: (username:string) => requests.get<Profil>(`/Profiles/VratiProfil/${username}`),
    getVodic: (username:string) => requests.get<Profil>(`/Vodic/VratiProfil/${username}`),
    uploadPhoto: (file:Blob) =>{
        let formData = new FormData();
        formData.append('File', file);
        return axios.post<Photo>('/Slike/DodajSliku', formData,{
            headers:{'Content-Type': 'multipart/form-data'}
        })
    },
    setMainPhoto: (id: string) => requests.post(`/Slike/${id}/setMain`, {}),
    deletePhoto: (id: string) =>requests.del(`/Slike/ObrisiSliku/${id}`),
    listTours: (username:string, predicate:string) => 
        requests.get<ObilazakKorisnika[]>(`/Profiles/VratiObilaskeKorisnika${username}/obilasci?predicate=${predicate}`)
}

const Rout ={
    listRoutsOfArea: (id:string,params:URLSearchParams) => axios.get<PaginatedResult<Ruta[]>>(`/Ruta/VratiRutePodrucja/${id}`,{params})
        .then(responseBody),
    create: (id:string,rout:RutaFormValues) => requests.post<void>(`/Ruta/KreirajRutu/${id}`,rout),
    curentRout: (id:string) => requests.get<Ruta>(`/Ruta/VratiRutu/${id}`)
}

const Admin = {
    list: () => requests.get<UserForAdmin[]>('/Administrator/VratiSveKorisnikeSaUlogama'),
    listRequestedGuides: () => requests.get<VodicZahtev[]>('/Administrator/zahtevi-vodica'),
    postGuide: (id:string) => requests.post<void>(`/Administrator/odobri-vodica/${id}`,{}),
    ban: (idKorisnika: string) => requests.post<UserForAdmin[]>(`/Administrator/BanujKorisnika/${idKorisnika}`, {})
};

const Follow = {
    updateFollowing: (username:string) => requests.post<void>(`/Follow/ZapratiOtprati/${username}`,{})
}

const agent = {
    Tours,
    Account,
    Plants,
    Areas,
    Profiles,
    Admin,
    Rout,
    Follow
}

export default agent;