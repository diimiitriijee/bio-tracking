import { Grid, Loader } from 'semantic-ui-react';
import TourList from './TourList';
import { observer } from 'mobx-react-lite';
import TourFilters from './TourFilters';
import { useStore } from '../../../stores/store';
import { PagingParams } from '../../../modules/Pagination';
import { useParams } from 'react-router-dom';
import InfiniteScroll from 'react-infinite-scroller';
import TourListItemPlaceholder from './TourListItemPlaceholder';
import { useEffect } from 'react';


export default observer(function ObilasciDashBoard() {

  const {tourStore:{setPagingParams,pagination,loadTours,loadingNext,setLoadingNext,loadAllTours,loadingInitial}} = useStore();
  const {id} = useParams();

  useEffect(() => {
    setPagingParams(new PagingParams(0));
    if(id) loadTours(id);

  }, [loadTours]);

  //Funkcija za paging da se svaki put na srolu ucitavaju po dva obilaska
  function handleGetNext(){
    setLoadingNext(true);
    setPagingParams(new PagingParams(pagination!.currentPage + 1));
    if(id===undefined)  loadAllTours().then(()=> setLoadingNext(false))
    else loadTours(id!).then(()=> setLoadingNext(false));
  }

  return (
    <Grid style={{marginTop:'10em'}}>
      <Grid.Column width='10'>
        {loadingInitial && !loadingNext ?(
          <>
            <TourListItemPlaceholder />
            <TourListItemPlaceholder />
          </>
        ):(
          <InfiniteScroll
          pageStart={0}
          loadMore={handleGetNext}
          hasMore={!loadingNext && !!pagination && pagination.currentPage < pagination.totalPages}
          initialLoad={false}
        >
          <TourList />
        </InfiniteScroll>
        )

        }
        
      </Grid.Column>
      <Grid.Column width='6'>
        <TourFilters />
      </Grid.Column>
      <Grid.Column width={10}>
        <Loader active={loadingNext} />
      </Grid.Column>
    </Grid>
  );
});
