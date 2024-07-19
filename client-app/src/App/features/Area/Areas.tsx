import { Container, Grid, Header } from "semantic-ui-react";
import PlantsDashBoard from "../Plants/PlantsDashBoard/PlantsDashBoard";
import AreaDetails from "./details/AreaDetails";
import LoadingComponent from "../../layout/LoadingComponent";
import { useParams } from "react-router-dom";
import { useStore } from "../../stores/store";
import { useEffect } from "react";
import { observer } from "mobx-react-lite";
import SearchBar from "../SerachBar/SearchBar";
import '../HomePage/Divider.css'
import SearchResultList from "../SerachBar/SearchResultList";
import { PagingParams } from "../../modules/Pagination";

export default observer( function Areas() {
    const { areaStore} = useStore();
    //const {plantStore:{setSelectedPlant}} = useStore();
    const {setSelectedArea, isSelected,loadingNext} = areaStore; 
    const {loadPlantsForSelectedTour, setPagingParams} = areaStore;
    const {id} = useParams();
    const { userStore: { isLoggedIn, isVodic } } = useStore();


    useEffect(() => {
      if (!isSelected) {
        setSelectedArea(id!);
      }
      loadPlantsForSelectedTour(id!);
      setPagingParams(new PagingParams(0));
      //setSelectedPlant(undefined);
      
  }, [setSelectedArea,loadPlantsForSelectedTour ,isSelected, id]);

  if (areaStore.isLoadingAreas && areaStore.isLoadingPlants && !loadingNext) return <LoadingComponent content='Ucitavanje biljaka' />
  return (
    <Container style={{ marginTop: '7em' }}>
      <Grid>
        <Grid.Row>
          <Grid.Column width={16}>
            <AreaDetails id={id} />
          </Grid.Column>
        </Grid.Row>
        <Grid.Row style={{justifyContent:"center"}} width='100%'>
          <Header  content='Lista biljaka koje rastu na ovom podrucju' as='h2'></Header>
        </Grid.Row>
        {isLoggedIn && isVodic && (
        <Grid.Row>
          <Grid.Column width={16}>
            <div className="searchBar" style={{ margin: '1em 0' }}>
              <div className="centered-content">
                <SearchBar />
                <div className="searchResults" style={{ marginTop: '1em' }}>
                  <SearchResultList />
                </div>
              </div>
            </div>
          </Grid.Column>
        </Grid.Row>
        )}
        <Grid.Row>
          <Grid.Column width={16}>
            <PlantsDashBoard />
          </Grid.Column>
        </Grid.Row>
      </Grid>
    </Container>
    
    
  )
})
