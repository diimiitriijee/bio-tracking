import  { useEffect } from 'react'
import {Container } from 'semantic-ui-react';
import ObilasciDashBoard from './DashBoard/ObilasciDashBoard';
import { useStore } from '../../stores/store';
import { observer } from 'mobx-react-lite';
import { useParams } from 'react-router-dom';
import AreaDetails from '../Area/details/AreaDetails';


function  Tours() {

    const {tourStore, areaStore} = useStore();
    const {loadTours,areaID, setAreaID,loadAllTours,cleanTourRegistry} = tourStore;
    const {setSelectedArea, isSelected, setIsSelected,unSelectAreas} = areaStore; 

    const {id} = useParams();

    useEffect(() => {
      unSelectAreas();

      //Ukoliko nemamo id onda prikazujemo sve obilaske
      if(id===undefined && areaID === undefined){
        loadAllTours();
      }
      else {
        //brisemo sve obilaske koji su prethodno postavljeni u bazi i ubacujemo nove
        cleanTourRegistry();
        setAreaID(id!);
        setSelectedArea(id!);
       // setIsSelected(true);
      }
  }, [loadTours, setSelectedArea, isSelected,setIsSelected, id]);

 

  return (
    <>
    <Container style = {{marginTop:'7em'}}>
      {areaID &&
        <AreaDetails id={areaID} />
      }
      
      <ObilasciDashBoard />
    </Container>
    </>
    
    
  )
}

export default  observer(Tours);