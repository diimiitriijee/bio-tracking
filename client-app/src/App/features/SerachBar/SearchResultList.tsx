import SearchResult from './SearchResult';
import "./SearchResultsList.css"
import { useStore } from '../../stores/store';
import { observer } from 'mobx-react-lite';

export default observer( function SearchResultList() {

  const {plantStore:{searchResults}} = useStore();
    if (!searchResults) {
        return 
    }
  return (
    <div className="results-list">
       {
        
        searchResults.map(( result, id)=>{
            return <SearchResult  result={result} key={id} />;
         })
       }
    </div>
  )
})
