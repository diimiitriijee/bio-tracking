import { makeAutoObservable, reaction, runInAction } from "mobx";
import { Podrucje, PodrucjeFormValue } from "../modules/Podrucje";
import agent from "../api/agent";
import { Biljka } from "../modules/Biljke";
import { toast } from "react-toastify";
import { Pagination } from "../modules/Pagination";
import { PagingParams } from "../modules/Pagination";
import { Koordinata } from "../modules/Koordinata";
export default class AreaStore{

    areaRegistry = new Map<string, Podrucje>();
    isLoading = false;
    selectedArea : Podrucje | undefined = undefined;
    isLoadingAreas: boolean = false;
    isSelected: boolean = false;
    isEditing: boolean = false;
    areaPlantRegistry = new Map<string,Biljka>();
    newPlantForArea:Biljka|undefined = undefined; 
    pagination: Pagination | null = null;
    pagingParams = new PagingParams();
    loadingNext = false;
    isLoadingPlants = false;
    newCoords:Koordinata[] = [];
    isCreatingArea = false;
    predicate = new Map().set('all',true);




    constructor(){
        makeAutoObservable(this);
        reaction(
            //reakcija na promenu kljuceva
            () => this.predicate.keys(),
            //reset zbog filtriranja, ako je prethodno selektovano is host kada se klikne na nesto drugo treba da krene od pocetne strane
            () => {
                this.pagingParams = new PagingParams();
                this.areaPlantRegistry.clear();
                this.loadPlantsForSelectedTour(this.selectedArea!.id);
            }
        )
    }

    setPredicate = (predicate: string, value: boolean) =>{
        //risetujemo predikat tako sto ga brisemo
        const resetPredicate =() =>{
            this.predicate.forEach((_value, key) =>{
                this.predicate.delete(key);
            })
        }
        //obradjeni slucajevi za svako filtriranje
        switch (predicate){
            case 'all':
                resetPredicate();
                this.predicate.set('all', value);
                break;
            case 'Lekovita':
                resetPredicate();
                this.predicate.set('Lekovita', value);
                break;
            case 'Nelekovita':
                resetPredicate();
                this.predicate.set('Nelekovita', value);
                break;
        }
    }

    setIsCreatingArea = (state : boolean) =>{
        this.isCreatingArea = state;
    }

    setNewCoords = (coords:Koordinata[] | undefined) =>{
        coords?.map(coord=>{
            this.newCoords?.push(coord);

        })
    }

    get getNewCoords(){
        return this.newCoords;
    }
    setLoadingNext=(state:boolean) =>{
        this.loadingNext = state;
    }

    setLoadingPlants=(state:boolean) =>{
        this.isLoadingPlants = state;
    }

   

    get getAreas(){
        return Array.from(this.areaRegistry.values());
    }   
    get allAreaPlants(){
        return Array.from(this.areaPlantRegistry.values());
    }
    setPagingParams=(pagingParams:PagingParams)=>{
        this.pagingParams = pagingParams;
    }

    get axiosParams(){
        const params = new URLSearchParams();
        params.append('pageNumber', this.pagingParams.pageNumber.toString());
        params.append('pageSize', this.pagingParams.pageSize.toString());
        this.predicate.forEach((value,key)=>{
            params.append(key, value);
        })
        return params;
    }


    get groupedPlants() {
        return Object.entries(
            this.allAreaPlants.reduce((plants, plant) => {
                const key = plant.lekovita ? 'Lekovite' : 'Nelekovite';
                plants[key] = plants[key] ? [...plants[key], plant] : [plant];
                return plants;
            }, {} as { [key: string]: Biljka[] })
        );
    }
    setAeraRegistry = (areas:Podrucje[]) => {
        if(this.areaRegistry.size>0)
            this.areaRegistry.clear();
        areas.forEach(area => {
            this.areaRegistry.set(area.id,area);
        });
    }

    loadAllAreas = async () =>{
        this.setLoadingInitial(true);
        try{
            const areas = await agent.Areas.list();
            this.setAeraRegistry(areas);


        }catch(error)
        {
            console.log(error);
        }finally{
            this.setLoadingInitial(false);
        }
    }
    createArea = async (area:PodrucjeFormValue) => {
        this.isLoading = true;
        try{
            await agent.Areas.create(area);
            //const newArea = new Podrucje(area);
            runInAction(()=>{
                this.areaRegistry.set(area.id,area);
                this.newCoords = [];
                this.isCreatingArea = false;
                this.isLoading = false;

            })
        }catch(error){
            console.log(error);
            this.isLoading = false;

        }
    }

    setLoadingInitial = (state:boolean)=>{
        this.isLoadingAreas = state;
    }

    setIsEditing = (value:boolean) =>{
        this.isEditing = value;
    }

    setIsSelected = (value:boolean) =>{
        this.isSelected = value;
    }

    setSelectedArea = async (id: string) => {
        this.isLoadingAreas = true;
        try {
            const area = await agent.Areas.current(id);
            runInAction(() => {
                this.selectedArea = area;
                this.isSelected = true;
            });
        } catch (error) {
            console.error(error);
            this.isSelected = false;
        } finally {
            runInAction(() => {
                this.isLoadingAreas = false;
            });
        }
    }

    unSelectAreas = () =>{
    
        this.isLoadingAreas = true;
        this.areaRegistry.clear();
        this.isLoadingAreas = false;
        this.areaPlantRegistry.clear();
    
    }

    setNewPlantForArea = (plant:Biljka) =>{
        this.newPlantForArea = plant;
    }

    removeNewPlantFromArea = () =>{
        this.newPlantForArea = undefined;
    }

    setPlantToArea = async () =>{
        this.setLoadingInitial(true);
        try{
            if(this.allAreaPlants.some(plant => plant.id === this.newPlantForArea!.id))
            {
                throw new Error();
            }
            else{
                await agent.Areas.addPlant(this.selectedArea!.id,this.newPlantForArea!);
                runInAction(()=>{
                    this.setAreaPlant(this.newPlantForArea!);
                })
            }
            

        }catch(error){
            toast.error('Biljka je vec dodata podrucju!');
        }
        finally{
            this.setLoadingInitial(false);
            this.newPlantForArea = undefined;
        }
    }

    private setAreaPlant = (plant: Biljka) => {
        console.log('ovo je biljka' + plant)
        this.areaPlantRegistry.set(plant.id, plant);
    }

    loadPlantsForSelectedTour = async (id:string) =>{
        this.setLoadingPlants(true);
        try {
            const result = await agent.Areas.plantList(id,this.axiosParams);
            console.log(result.pagination)
            
            result.data.forEach((plant:Biljka) => {
                    this.setAreaPlant(plant);
                });
                this.setPagination(result.pagination);
                this.setLoadingPlants(false);

                
        } catch (error) {
            console.log(error);
            this.setLoadingPlants(false);            
        }
    }

    setPagination = (pagination:Pagination) =>{
        this.pagination = pagination
    }

    deleteArea = async (id:string) =>{
        this.setLoadingInitial(true);
        try{
            await agent.Areas.delete(id);
            runInAction(()=>{
                this.areaRegistry.delete(id);
            })

        }catch(error)
        {
            console.log(error);
        } finally{
            this.setLoadingInitial(false);
        }
    }

    deletePlantFromArea = async (idPlant:string)=>{
        this.isLoading = true;
        try{
            await agent.Areas.deletePlant(this.selectedArea!.id,idPlant);
            runInAction(()=>{
                this.setLoadingInitial(true);
                this.areaPlantRegistry.delete(idPlant);
                this.setLoadingInitial(false);
            })
        }catch(error){
            console.log(error);
        }finally{
            this.isLoading = false;
        }
    }
}