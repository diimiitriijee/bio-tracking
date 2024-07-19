import { useEffect } from 'react';
import { Form, Grid, Segment, Checkbox, Button, Container, Divider } from 'semantic-ui-react';
import Map from '../Map/Map';
import "./Divider.css";
import { useStore } from '../../stores/store';
import SearchBar from '../SerachBar/SearchBar';
import { observer } from 'mobx-react-lite';
import SearchResultList from '../SerachBar/SearchResultList';
import { NavLink } from 'react-router-dom';

export default observer(function HomeDivider() {
  const { plantStore } = useStore();
  const { isMedical, setMedical } = plantStore;
  const { plantStore: { selectedPlant, deletePlant, isLoading } } = useStore();
  const { areaStore: { setIsSelected, loadAllAreas, isLoadingAreas, unSelectAreas } } = useStore();
  const { userStore: { isLoggedIn, isAdmin, isVodic } } = useStore();

  //if (plantStore.loadingInitial) return <LoadingComponent content='Loading biljkee' />
  useEffect(() => {
    setIsSelected(false);
    unSelectAreas();
  }, [setIsSelected]);

  return (
    <Segment placeholder className='segmentClass'>
      <Grid columns={2} relaxed='very' stackable>
        <Grid.Column verticalAlign='middle'>
          <Map />
        </Grid.Column>

        <Grid.Column verticalAlign={isLoggedIn? 'top' : 'middle'}>
          <Form>
            <Container>
              {isLoggedIn && (isVodic || isAdmin) ?(
                <>
                  <Divider horizontal>Područja</Divider>
                  <Grid columns={2} relaxed='very'>
                    <Grid.Row>
                      <Grid.Column>
                        <Button positive fluid loading={isLoadingAreas} content='Prikazi sva podrucja' onClick={loadAllAreas} />
                      </Grid.Column>
                      {isVodic && (
                        <Grid.Column>
                        <Button positive fluid content='Kreiraj podrucje' as={NavLink} to='/createArea' />
                      </Grid.Column>
                      )}
                      
                    </Grid.Row>
                  </Grid>
                  <br/><br/>
                  <Divider horizontal>Biljke</Divider>
                </>
                )  :
                (
                  <>
                    <Divider horizontal>Područja</Divider>
                      <Grid columns={1} relaxed='very'>
                        <Grid.Row>
                          <Grid.Column>
                            <Button positive fluid loading={isLoadingAreas} content='Prikazi sva podrucja' onClick={loadAllAreas} />
                          </Grid.Column>
                        </Grid.Row>
                      </Grid>
                      <br/><br/>
                      <Divider horizontal>Biljke</Divider>
                  </>
                )
             }
                  <div className="searchBar">
                    <div className="centered-content">
                      <SearchBar />
                      <div className='searchResults'>
                        <SearchResultList />
                      </div>
                      <Checkbox toggle label="Lekovite" checked={isMedical} onChange={() => setMedical(!isMedical)} />
                    </div>
                  </div><br/><br/><br/><br/><br/><br/>
              {(isLoggedIn && (isVodic || isAdmin)) &&
                <>
                  <Grid columns={3} relaxed='very' align-self='flex-end'>
                    <Grid.Row>
                      <Grid.Column>
                        <Button positive fluid content='Dodaj biljku' as={NavLink} to='/createPlant' />
                      </Grid.Column>
                      <Grid.Column>
                        <Button disabled={!selectedPlant} fluid content='Izmeni biljku' as={NavLink} to={`/managePlant/${selectedPlant?.id}`} color='orange' />
                      </Grid.Column>
                      <Grid.Column>
                        <Button disabled={!selectedPlant} fluid loading={isLoading} content='Obrisi biljku' color='red' onClick={() => deletePlant(selectedPlant!.id)} />
                      </Grid.Column>
                    </Grid.Row>
                  </Grid>
                </>
              }
            </Container>
          </Form>
        </Grid.Column>
      </Grid>
    </Segment>
  );
});
