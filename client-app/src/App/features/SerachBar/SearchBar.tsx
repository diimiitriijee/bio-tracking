import { useEffect, useState } from 'react';
import { FaSearch } from 'react-icons/fa';
import "./search-bar.css";
import { Biljka } from '../../modules/Biljke';
import { useStore } from '../../stores/store';
import { observer } from 'mobx-react-lite';

export default observer(function SearchBar() {
  const { plantStore, areaStore } = useStore();
  const { setSearchResult, getPlants, selectedPlant, setSearchValue, plantRegistry, loadPlants ,unSelectPlant} = plantStore;
  const {unSelectAreas, removeNewPlantFromArea} = areaStore;
  const [inputValue, setInputValue] = useState('');

  useEffect(() => {
    
    unSelectPlant();
    removeNewPlantFromArea();
    
  }, []);

  useEffect(() => {
     loadPlants();
  }, [plantRegistry.size, loadPlants]);

  useEffect(() => {
    if (selectedPlant) {
      console.log('usa sam odje')
      setInputValue(selectedPlant.naziv);
     } else{
       setInputValue('');
     setSearchValue('');
     }
  }, [selectedPlant]);

  const handleChange = (value: string) => {
    setInputValue(value);
    setSearchValue(value);
    filterResults(value);
  };

  const filterResults = (value: string) => {
    
    //za svaku promenu 
    setSearchResult(undefined);
    unSelectPlant();
    unSelectAreas();
    removeNewPlantFromArea();
     if (value.trim() !== '') {
      const filteredResults: Biljka[] = getPlants?.filter((biljka: Biljka) =>
        biljka.naziv.toLowerCase().includes(value.toLowerCase())
      ) || [];
      setSearchResult(filteredResults);
    }
  };

  

  return (
    <div className='input-wrapper'>
      <FaSearch id="search-icon" />
      <input
        style={{ border: 0 }}
        placeholder="Pretrazite biljku ..."
        onChange={(e) => handleChange(e.target.value)}
        value={inputValue}
      />
      
    </div>
  );
});
