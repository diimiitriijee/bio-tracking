import { makeAutoObservable, runInAction } from "mobx";
import { Biljka, BiljkaFormValues } from "../modules/Biljke";
import agent from "../api/agent";
import { Podrucje } from "../modules/Podrucje";
import { store } from "./store";

export default class PlantStore {

    plantRegistry = new Map<string, Biljka>();
    loadingInitial = false;
    isLoading = false;
    isMedical = false;
    searchResults:Biljka[] | undefined = undefined;
    selectedPlant: Biljka | undefined = undefined;
    searchValue: string = '';
    
    constructor(){
        makeAutoObservable(this);
    }
    

    setMedical =(state:boolean) => {
        this.isMedical = state;
    }
    
    setSelectedPlant = async (plant:Biljka) =>{
        store.areaStore.setLoadingInitial(true);
        try{
            runInAction(()=>{
                this.selectedPlant = plant;
                this.setSearchValue(plant!.naziv);
                this.searchResults = undefined;
            })
            const podrucja : Podrucje[] = await agent.Plants.listAreas(plant!.id);
            runInAction(()=>{
                store.areaStore.setAeraRegistry(podrucja);
            })
            
            console.log(podrucja)

        }catch(error)
        {
            console.log(error);
            
        }
        finally{
            store.areaStore.setLoadingInitial(false);
        }
        
    }

    unSelectPlant = () =>{
        store.areaStore.setLoadingInitial(true);
        this.selectedPlant = undefined;
        store.areaStore.setLoadingInitial(false);
    }

    setSearchValue = (value: string) => {
        this.searchValue = value;
    }

    setLoadingInitial = (state: boolean) =>{
        this.loadingInitial = state;
    }

    setSearchResult = (plants:Biljka[] | undefined)=>{
        this.searchResults = plants;
    }

    private getPlant = (id:string) =>{
        return this.plantRegistry.get(id);
    }

    get getPlants(){
        return this.isMedical? 
        Array.from(this.plantRegistry.values()).filter(plant => plant.lekovita)
        :Array.from(this.plantRegistry.values());
    }

    

    setPlant = (plant: Biljka) => {
        this.plantRegistry.set(plant.id, plant);
    }

    loadPlants = async () => {
        this.setLoadingInitial(true);
        try {
            const plants :Biljka[] = await agent.Plants.list();
            
                plants.forEach(plant => {
                    this.setPlant(plant);
                });
                this.setLoadingInitial(false);
                
        } catch (error) {
            console.log(error);
           
                this.setLoadingInitial(false);
            
        }
    }

    loadPlant = async (id:string) => {
        let plant = this.getPlant(id);
        console.log(plant)
        //Ako biljka postoji u registru ucitava je, ako ne postoji cita iz baze
        if(plant){

            runInAction(() =>{
                this.selectedPlant = plant;
            })
            console.log(this.selectedPlant);
            return plant;
        }
        else{
            this.setLoadingInitial(true);
            try{
                plant = await agent.Plants.curentPlant(id);
                runInAction(()=>{
                    this.selectedPlant = plant;
                   
                })
                this.setLoadingInitial(false);
                return plant;
            }catch(error){
                console.log(error);
                this.setLoadingInitial(false);
            }
        }
    }

    createPlant = async (plant:FormData,newplant:BiljkaFormValues) =>{
        this.isLoading = true;
        try{
            console.log(plant);
            await agent.Plants.create(plant);
             
        }catch(error){
            console.log(error);
        }
        finally{
            this.isLoading = false;
        }
    }

    updatePlant = async (plant:BiljkaFormValues)=>{
        try{
            await agent.Plants.update(plant)
            runInAction(()=>{
                if(plant.id){
                    const updatedTour = {...this.getPlant(plant.id),...plant};
                    this.plantRegistry.set(plant.id,updatedTour as Biljka);
                    this.selectedPlant = updatedTour as Biljka;

                }
            })

        }catch(error){
            console.log(error);
        }
    }    

    deletePlant = async(id:string) =>{
        this.isLoading = true;
        try{
            await agent.Plants.delete(id);
            runInAction(()=>{
                this.plantRegistry.delete(id);
                this.selectedPlant = undefined;
            }) 

        } catch(error){

        }finally{
            this.isLoading = false;
        }
    }

}