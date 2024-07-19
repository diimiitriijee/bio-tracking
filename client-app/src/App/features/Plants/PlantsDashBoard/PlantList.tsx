import { Fragment } from 'react'
import { useStore } from '../../../stores/store';
import { Header } from 'semantic-ui-react';
import PlantListItem from './PlantListItem';
import { observer } from 'mobx-react-lite';
import LoadingComponent from '../../../layout/LoadingComponent';

export default observer( function PlantList() {
    const {areaStore} = useStore();
    const {groupedPlants, isLoadingAreas} = areaStore;
    console.log(groupedPlants)
    if(isLoadingAreas) return <LoadingComponent content='Ucitavanje biljaka...' />
    return (
    <>
    {
        groupedPlants.map(([group, plants])=>(
            <Fragment key={group}>
                <Header sub color='teal'>
                    {group}
                </Header>
                
                    {
                        plants.map(plant => (
                            
                            <PlantListItem key={plant.id} plant={plant}/>
                        ))
                    }
     
            </Fragment>
        ))
    }
    </>
    )
})
