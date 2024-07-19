import "./SearchResult.css"
import { Biljka } from '../../modules/Biljke'
import { useStore } from "../../stores/store"
import { observer } from "mobx-react-lite"

interface Props {
  result : Biljka
}

export default observer( function SearchResult({result}:Props) {

  const {plantStore:{setSelectedPlant},areaStore:{setNewPlantForArea,isSelected}} = useStore();

  const handleOnClick= () =>{
    if(isSelected){
      setNewPlantForArea(result);
      setSelectedPlant(result);
    }
    else setSelectedPlant(result);
  } 

  return (
    <div className='searchResult' onClick={handleOnClick}>{result.naziv}</div>
  )
})
