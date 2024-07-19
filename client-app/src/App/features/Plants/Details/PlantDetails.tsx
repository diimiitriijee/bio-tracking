import { Grid } from "semantic-ui-react";
import PlantDetailedHeader from "./PlantDetailsHeader";
import { useParams } from "react-router-dom";
import { useEffect } from "react";
import { useStore } from "../../../stores/store";
import PlantDetailedInfo from "./PlantDetailedInfo";
import { observer } from "mobx-react-lite";
import PlantListItemPlaceholder from "../PlantsDashBoard/PlantListItemPlaceholder";

export default observer( function PlantDetails() {

    const {id} = useParams();
    const {plantStore:{loadPlant, selectedPlant:plant,loadingInitial}} = useStore();

    useEffect(()=>{

      if(id) {
        loadPlant(id);
      }
    },[id, loadPlant])
    if(loadingInitial || !plant) return <PlantListItemPlaceholder/>;

  return (
    <Grid >
        <Grid.Column width ={10} style={{marginTop:'10px'}}>
        <PlantDetailedHeader plant={plant!}/>
        <PlantDetailedInfo plant={plant!} />
        </Grid.Column>
    </Grid>
  )
})
