import { Item, Segment, Header, Button } from 'semantic-ui-react'
import { useStore } from '../../../stores/store';
import Map from '../../Map/Map'
import { observer } from 'mobx-react-lite';
import { Link, useNavigate } from 'react-router-dom';
import { ToastContainer } from 'react-toastify';

interface Props{
    id: string | undefined;
}
export default observer( function  AreaDetails({id}:Props) {

    
    const {areaStore:{selectedArea,newPlantForArea, setPlantToArea,isLoadingAreas, deleteArea}} = useStore();
    const { userStore: { isLoggedIn, isVodic } } = useStore();
    const navigate = useNavigate();


  return (
    <>
    <ToastContainer position="top-right" autoClose={5000} hideProgressBar={false} />
    <Segment basic attached='top' style={{padding: '0'}}>
        <Segment basic>
                    <Item.Group>
                        <Item>
                            <Item.Content style={{textAlign:'center'}}>
                                <Header
                                    content={selectedArea?.oblast}
                                    
                                />
                                
                            </Item.Content>
                        </Item>
                    </Item.Group>
                </Segment>
                <Segment clearing>
                    <Map />
                    <Item.Group>
                        <Item>
                            <Item.Content style={{textAlign:'center'}}>
                            {isLoggedIn && isVodic && (
                                <>
                                    <Button positive content='Dodaj obilazak' as={Link} to='/createTour' />
                                    <Button color='red' content='Obrisi podrucje' onClick={() => deleteArea(selectedArea!.id).then(()=>navigate('/'))} />

                                    <Button as={Link} to={'/createRout'} color='green' floated='right'>
                                        Dodaj rutu
                                    </Button>
                                    
                            <Button disabled={!newPlantForArea} loading={isLoadingAreas} onClick={setPlantToArea} color='green' floated='right'>
                                Dodaj biljaku podrucju
                            </Button>
                                  </>  
                                
                                )}
                            <Button as={Link} to={`/areaRouts/${id}`} color='teal' floated='right'>
                                Pregled ruta
                            </Button>
                            <Button as={Link} to={`/areaDetails/${id}`} color='teal' floated='right'>
                                Pregled biljaka
                            </Button>
                            
                            
                            </Item.Content>
                        </Item>
                    </Item.Group>
                    
                </Segment>
                
                
            </Segment>
            </>
  )
})
