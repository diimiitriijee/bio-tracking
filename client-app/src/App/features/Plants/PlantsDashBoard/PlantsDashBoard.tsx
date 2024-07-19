import { Grid, Loader } from 'semantic-ui-react'
import PlantList from './PlantList'
import { observer } from 'mobx-react-lite'
import { useStore } from '../../../stores/store'
import { useParams } from 'react-router-dom'
import { PagingParams } from '../../../modules/Pagination'
import InfiniteScroll from 'react-infinite-scroller'
import PlantListItemPlaceholder from './PlantListItemPlaceholder'
import PlantsFilter from './PlantsFilter'

export default observer( function PlantsDashBoard() {
  const { areaStore:{loadPlantsForSelectedTour,pagination,setPagingParams,loadingNext,setLoadingNext,isLoadingPlants}} = useStore();
  const {id} = useParams();
  

  function handleGetNext(){
    setLoadingNext(true);
    setPagingParams(new PagingParams(pagination!.currentPage + 1));
    loadPlantsForSelectedTour(id!).then(()=> setLoadingNext(false));
  }
  console.log('pagination total page: '+pagination?.totalPages)
  return (
    <Grid>
      <Grid.Column width='10'>
        {isLoadingPlants && !loadingNext ?
          (<>
            <PlantListItemPlaceholder />
            <PlantListItemPlaceholder />

          </>):
          (

          <InfiniteScroll
            pageStart={0}
            loadMore={handleGetNext}
            hasMore={!loadingNext && !!pagination && pagination.currentPage < pagination.totalPages}
            initialLoad={false}
          >
          <PlantList />
        </InfiniteScroll>
          )
        }
      
        
      </Grid.Column>
      <Grid.Column width='6'>
        <PlantsFilter />
      </Grid.Column>
      <Grid.Column width={10}>
        <Loader active={loadingNext} />
      </Grid.Column>
    </Grid>
  )
})
