import { makeAutoObservable, reaction, runInAction } from "mobx";
import { Obilazak, ObilazakFromValues } from "../modules/Obilazak";
import agent from "../api/agent";
import { format } from "date-fns";
import { store } from "./store";
import { Profil } from "../modules/Profil";
import { Pagination, PagingParams } from "../modules/Pagination";
import { Ruta } from "../modules/Ruta";

export default class TourStore{
    
    tourRegistry = new Map<string,Obilazak>();
    selectedTour:Obilazak | undefined = undefined;
    editMode = false;
    loading = false;
    loadingInitial = false;
    pagination: Pagination | null = null;
    pagingParams = new PagingParams();
    loadingNext = false;
    predicate = new Map().set('all',true);
    areaID : string|undefined = undefined;   
    isCreatingTour = false; 
    isSubmiting = false;
    tourRout: Ruta | undefined = undefined;
    showRout = false;



    constructor(){
        makeAutoObservable(this)

        reaction(
            //reakcija na promenu kljuceva
            () => this.predicate.keys(),
            //reset zbog filtriranja, ako je prethodno selektovano is host kada se klikne na nesto drugo treba da krene od pocetne strane
            () => {
                this.pagingParams = new PagingParams();
                this.tourRegistry.clear();
                if(this.areaID === undefined) this.loadAllTours();
                else this.loadTours(this.areaID!);
            }
        )
    }

    setIsCreateingTour = (state:boolean) =>{
        this.isCreatingTour = state;
    }

    setLoadingNext =(state:boolean)=>{
        this.loadingNext = state;  
    }

    setShowRout= (state : boolean) =>{
        this.showRout = state;
    }

    cleanTourRegistry=()=>{
        this.tourRegistry.clear();
    }

    setPagingParams=(pagingParams:PagingParams)=>{
        this.pagingParams = pagingParams;
    }

    setPredicate = (predicate: string, value: string | Date) =>{
        const resetPredicate =() =>{
            this.predicate.forEach((_value, key) =>{
                if(key !== 'startDate') this.predicate.delete(key);
            })
        }
        //obradjeni slucajevi za svako filtriranje
        switch (predicate){
            case 'all':
                resetPredicate();
                this.predicate.set('all', true);
                break;
            case 'isGoing':
                resetPredicate();
                this.predicate.set('isGoing', true);
                break;
            case 'isHost':
                resetPredicate();
                this.predicate.set('isHost', true);
                break;
            case 'startDate':
                this.predicate.delete('startDate');
                this.predicate.set('startDate', value);
        }
    }

    get axiosParams(){
        const params = new URLSearchParams();
        params.append('pageNumber', this.pagingParams.pageNumber.toString());
        params.append('pageSize', this.pagingParams.pageSize.toString());
        this.predicate.forEach((value,key)=>{
            if(key === 'startDate'){
                params.append(key,(value as Date).toISOString())
            } else{
                params.append(key, value);
            }
        })
        return params;
    }

    get toursByDate(){
        return Array.from(this.tourRegistry.values()).sort((a,b)=> 
            a.datumOdrzavanja!.getTime()- b.datumOdrzavanja!.getTime());
    }

    get groupedTours(){
        return Object.entries(
            this.toursByDate.reduce((tours, tour) =>{
                const date = format(tour.datumOdrzavanja!,'dd MMM yyyy');
                tours[date] = tours[date] ? [...tours[date], tour]:[tour];
                return tours;
            },{} as {[key:string]:Obilazak[]})
        )
    }

    setAreaID = (id:string|undefined) =>{
        this.areaID = id; 
    }

    setLoadingInitial = (state:boolean)=>{
        
        this.loadingInitial = state;
    }

    
    
    loadTours = async (id:string)=>{
        this.setLoadingInitial(true);
        try{
            const result = await agent.Tours.list(id, this.axiosParams);
                result.data.forEach(tour=>{
                    this.setTour(tour);
                })
                this.setPagination(result.pagination);
                this.setLoadingInitial(false);
                
        }catch(error){
            console.log(error);
            this.setLoadingInitial(false);

        }
    }

    loadAllTours = async () =>{
        this.setLoadingInitial(true);
        try{
            const result = await agent.Tours.listAllTours(this.axiosParams);
                
                result.data.forEach(tour=>{
                    this.setTour(tour);
                })
                this.setPagination(result.pagination);
                this.setLoadingInitial(false);
                
        }catch(error){
            console.log(error);
            this.setLoadingInitial(false);

        }
    }

    setPagination = (pagination:Pagination) =>{
        this.pagination = pagination
    }

    loadTour = async(id:string)=>{
            //ucitavanje obilaska
            
                this.setLoadingInitial(true);
                try{
                    let tour = await agent.Tours.details(id);
                    let rout = await agent.Rout.curentRout(tour.ruta.id);
                    console.log(tour)
                    this.setTour(tour);
                    runInAction(()=>{
                        this.selectedTour = tour;
                        this.tourRout = rout;
                    })
                    this.setLoadingInitial(false);
                    return tour;
                }catch(error){
                    console.log(error);
                    this.setLoadingInitial(false);
                }
            
        
    }

    private setTour = (tour:Obilazak)=>{
        const user = store.userStore.user;
        if(user){
            tour.ide = tour.ucesnici!.some(
                a => a.username === user.userName
            )
                    

            tour.isVodic = tour.vodic?.username === user.userName;
        }
        tour.datumOdrzavanja = new Date(tour.datumOdrzavanja!);
        this.tourRegistry.set(tour.id,tour);
    }

    private getTour = (id:string) =>{
        return this.tourRegistry.get(id);
    }



    createTour = async (areaId:string,routId:string, tour:ObilazakFromValues) => {
        
        this.isSubmiting = true;
        try{
            await agent.Tours.createTourWithRout(areaId,routId,tour);
            const newTour = new Obilazak(tour);
            this.setTour(newTour);
            runInAction(()=>{
                this.selectedTour = newTour;
                this.isSubmiting = false;

            })
        }catch(error){
            console.log(error);
            runInAction(()=>{
                this.isSubmiting = false;
            })
        }
    }

    updateTour = async (tour:ObilazakFromValues) =>{
        try{
            await agent.Tours.update(tour);
            runInAction(()=>{
                if(tour.id){
                    const updatedTour = {...this.getTour(tour.id),...tour};
                    this.tourRegistry.set(tour.id,updatedTour as Obilazak);
                    this.selectedTour = updatedTour as Obilazak;

                }
                
            })
        }catch(error){
            console.log(error);
      
        }
    }

    deleteTour = async (id:string) =>{
        this.loadingInitial = true;
        try{
            await agent.Tours.delete(id);
            runInAction(()=>{
                this.tourRegistry.delete(id);
                this.loadingInitial = false;
            })
        }catch(error){
            console.log(error);
            runInAction(()=>{
                this.loadingInitial = false;
            })
        }
    }

    closeForm =() =>{
        this.selectedTour=undefined;

    }

    updateAttendance = async () =>{
        const user = store.userStore.user;
        runInAction(()=>this.loading = true)
        try{
                if(this.selectedTour?.ide){
                    await agent.Tours.cancleAttend(this.selectedTour!.id);
                    runInAction(()=>{
                    this.selectedTour!.ucesnici =
                        this.selectedTour!.ucesnici?.filter(u=> u.username !== user?.userName);
                    this.selectedTour!.ide = false;
                })
                }
                else{
                    await agent.Tours.attend(this.selectedTour!.id);
                    runInAction(()=>{
                    const attendee = new Profil(user!);
                    this.selectedTour?.ucesnici?.push(attendee);
                    this.selectedTour!.ide = true;
                })
                }
                this.tourRegistry.set(this.selectedTour!.id,this.selectedTour!);
           
        }catch(error){
            console.log(error);
        } finally{
            runInAction(()=>this.loading = false)
        }
    }

    cancleTour = async (id:string) =>{
        this.loading = true;
        try{
            await agent.Tours.delete(id);
            runInAction(()=>{
                this.tourRegistry.delete(this.selectedTour!.id);
                this.loading = false;
            })

        }catch(error){
            console.log(error);
            runInAction(() => this.loading = false);
        }
       
    }

    clearSelectedTour = () => {
        this.selectedTour = undefined;
    }

    handleRateGuide = async (tourId: string, rating: number, comment: string) => {
        this.loading = true;
        try {
            await agent.Tours.rateGuide(tourId, rating, comment);
            runInAction(() => {
                // You might want to update the local state if needed
                const tour = this.tourRegistry.get(tourId);
                if (tour) {
                    // Example: Update the tour with new rating details if your API returns them
                    // tour.guideRating = rating;
                    // tour.guideComment = comment;
                    this.tourRegistry.set(tourId, tour);
                }
                this.loading = false;
            });
        } catch (error) {
            console.log(error);
            runInAction(() => {
                this.loading = false;
            });
        }
    };
}