import { Header } from 'semantic-ui-react'
import { useStore } from '../../../stores/store';
import { observer } from 'mobx-react-lite';
import TourListItem from './TourListItem';
import { Fragment } from 'react/jsx-runtime';


export default observer(function TourList() {
    const {tourStore} = useStore();
    const {groupedTours} = tourStore; 
    console.log(groupedTours)
    return (
    <>
    {
        groupedTours.map(([group, tours])=>(
            <Fragment key={group}>
                <Header sub color='teal'>
                    {group}
                </Header>
                
                    {
                        tours.map(tour => (
                            <TourListItem key={tour.id} tour={tour}/>
                        ))
                    }
     
            </Fragment>
        ))
    }
    </>
    /*
    <Segment>
        <Item.Group divided>
            {
                toursByDate.map(tour => (
                    <TourListItem key={tour.id} tour={tour}/>
                ))
            }
        </Item.Group>
    </Segment>
*/

)
})
