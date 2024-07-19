import { makeAutoObservable, runInAction } from "mobx";
import { Ruta, RutaFormValues } from "../modules/Ruta";
import agent from "../api/agent";
import { Koordinata } from "../modules/Koordinata";
import { Pagination, PagingParams } from "../modules/Pagination";


export default class RoutStore{

    routRegistry = new Map<string,Ruta>();
    loadingRouts = false;
    isCreating = false;
    isCreatingRout = false;
    newCoords:Koordinata[] = [];
    pagination: Pagination | null = null;
    pagingParams = new PagingParams();
    loadingNext = false;
    selectedRoutId : string | undefined = undefined;
    selectedRout: Ruta | undefined = undefined;
    routLength: number | undefined = undefined;
    isTourForm: boolean = false;

    constructor(){
        makeAutoObservable(this);
    }
    get routs(){
        return Array.from(this.routRegistry.values());
    }

    get axiosParams(){
        const params = new URLSearchParams();
        params.append('pageNumber', this.pagingParams.pageNumber.toString());
        params.append('pageSize', this.pagingParams.pageSize.toString());
        
        return params;
    }

    clearSelectedRout  = () =>{
        this.selectedRout= undefined;
    }
    clearRoutRegistry = () =>{
        this.routRegistry.clear();
    }
    setIsTourForm = (state:boolean) =>{
        this.isTourForm = state;
    }
    setRoutLength = (length:number) =>{
        this.routLength = length;
    }

    setSelectedRoutId = (id:string | undefined) =>{
        this.selectedRoutId = id;
    }

    get groupedRouts(){
        return Object.entries(
            this.routs.reduce((routs, rout) =>{
                routs[rout.tip] = routs[rout.tip] ? [...routs[rout.tip], rout]:[rout];
                return routs;
            },{} as {[key:string]:Ruta[]})
        )
    }
    

    setPagingParams=(pagingParams:PagingParams)=>{
        this.pagingParams = pagingParams;
    }

    setLoadingNext =(state:boolean)=>{
        this.loadingNext = state;  
    }

    setNewCoords = (coords:Koordinata[] | undefined) =>{
        coords?.map(coord=>{
            this.newCoords?.push(coord);

        })
    }
    get getNewCoords(){
        return this.newCoords;
    }
    setRoutRegistry = (routs:Ruta[]) =>{
        routs.map(rout=> this.routRegistry.set(rout.id,rout));
    }
    setIsCreatingRout = (state:boolean) =>{
        this.isCreatingRout = state;
    }
    setPagination = (pagination:Pagination) =>{
        this.pagination = pagination
    }
    loadRouts = async (id:string) =>{
        this.loadingRouts = true;
        try{
            const routs = await agent.Rout.listRoutsOfArea(id,this.axiosParams);
            runInAction(()=>{
                this.setRoutRegistry(routs.data);
                this.loadingRouts = false;
                this.setPagination(routs.pagination);

            })
        } catch(error){
            console.log(error);
            this.loadingRouts = false;
        }
        console.log(this.loadingRouts)
    }
    private getRout = (id:string) =>{
        return this.routRegistry.get(id);
    }
    loadRout = async (id:string) =>{
        let rout = this.getRout(id);
        console.log(rout)
        //Ako ruta postoji u registru ucitava je, ako ne postoji cita iz baze
        if(rout){

            runInAction(() =>{
                this.selectedRout = rout;
            })
            console.log(this.selectedRout);
            return rout;
        }
        else{
            this.loadingRouts = true;
            try{
                rout = await agent.Rout.curentRout(id);
                runInAction(()=>{
                    this.selectedRout = rout;
                   
                })
                this.loadingRouts=false;
                return rout;
            }catch(error){
                console.log(error);
                this.loadingRouts=false;
            }
        }
    }

    createRout = async (rout:RutaFormValues,areaID:string) => {
        this.isCreating = true;
        try{
            await agent.Rout.create(areaID,rout);
            const newRout = new Ruta(rout);
            runInAction(()=>{
                this.routRegistry.set(newRout.id,newRout);
                this.isCreatingRout = false;
                this.isCreating = false;
            })
        } catch (error){
            console.log(error);
            runInAction(()=>{
                this.isCreatingRout = false;
                this.isCreating = false;
            })
        }
    }
    
}