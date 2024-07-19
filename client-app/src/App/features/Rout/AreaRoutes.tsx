import { Container, Grid, Header } from "semantic-ui-react";
import AreaDetails from "../Area/details/AreaDetails";
import { useParams } from "react-router-dom";
import RoutDashBoard from "./RoutDashBoard/RoutDashBoard";
import { useEffect } from "react";
import { useStore } from "../../stores/store";

export default function AreaRoutes() {

  const {routStore:{clearRoutRegistry}} = useStore();
  const {id} = useParams();

  useEffect(()=>{
    return ()=>clearRoutRegistry();
  },[clearRoutRegistry])

  return (
    <Container style={{ marginTop: '7em' }}>
      <Grid>
        <Grid.Row>
          <Grid.Column width={16}>
            <AreaDetails id={id} />
          </Grid.Column>
        </Grid.Row>
        <Grid.Row style={{justifyContent:"center"}} width='100%'>
          <Header  content='Lista ruta' as='h2'></Header>
        </Grid.Row>
        <Grid.Row>
          <Grid.Column width={16}>
            <RoutDashBoard />
          </Grid.Column>
        </Grid.Row>
      </Grid>
    </Container>
  )
}
