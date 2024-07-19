import { observer } from "mobx-react-lite"
import { Grid } from "semantic-ui-react"
import { useStore } from "../../../stores/store";
import RoutDetailedHeader from "./RoutDetailedHeader";
import RoutDetailedInfo from "./RoutDetailedInfo";
import { useEffect } from "react";
import { useParams } from "react-router-dom";
import RoutListItemPlaceholder from "../RoutDashBoard/RoutListItemPlaceholder";

export default observer( function RoutDetails() {

  const {id} = useParams();
  const {routStore:{loadRout, selectedRout:ruta,loadingRouts,clearSelectedRout}, tourStore:{setShowRout}} = useStore();
  

  useEffect(()=>{
    setShowRout(true);

    if(id) {
      loadRout(id);
      setShowRout(true);
    }
    return () => {
      clearSelectedRout();
      setShowRout(false);
  }
  },[id, loadRout])
  if(loadingRouts || !ruta) return <RoutListItemPlaceholder/>;

  return (
    <Grid style={{ marginTop: '7em' }}>
        <Grid.Column width ={16}>
            <RoutDetailedHeader rout={ruta} />
            <RoutDetailedInfo rout={ruta} />
        </Grid.Column>
        
    </Grid>
  )
})
