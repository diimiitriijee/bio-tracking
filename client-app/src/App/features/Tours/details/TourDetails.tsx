import { Grid} from 'semantic-ui-react'
import { useStore } from '../../../stores/store'
import { observer } from 'mobx-react-lite'
import { useParams } from 'react-router-dom';
import { useEffect } from 'react';
import TourDetailedHeader from './TourDetailedHeader';
import TourDetailedInfo from './TourDetailedInfo';
import TourDetailedChat from './TourDetailedChat';
import TourDetailedSidebar from './TourDetailedSidebar';
import TourDetailsPlaceHolder from './TourDetailsPlaceHolder';


export default observer( function TourDetails() {

    const {tourStore} = useStore();
    const {selectedTour : tour,loadTour,loadingInitial, clearSelectedTour,setShowRout} = tourStore;

    const {id} = useParams();

    useEffect(()=>{
        setShowRout(true);
        //ucitavanje obilaskska
        if(id) loadTour(id);
        return () => {
            clearSelectedTour();
            setShowRout(false);
        }
    },[id, loadTour, clearSelectedTour])

    if(loadingInitial || !tour) return <TourDetailsPlaceHolder />;
    console.log(tour)

  return (

    <Grid style={{ marginTop: '7em' }}>
        <Grid.Column width ={10}>
            <TourDetailedHeader tour={tour}/>
            <TourDetailedInfo tour={tour}/>
            <TourDetailedChat obilazakId={tour.id}/>
        </Grid.Column>
        <Grid.Column width={6}>
            <TourDetailedSidebar attendees={tour.ucesnici!} />
        </Grid.Column>
    </Grid>
)
})
