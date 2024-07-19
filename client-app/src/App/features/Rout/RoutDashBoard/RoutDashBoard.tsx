import InfiniteScroll from "react-infinite-scroller";
import { Grid, Loader } from "semantic-ui-react";
import { useStore } from "../../../stores/store";
import { useParams } from "react-router-dom";
import { PagingParams } from "../../../modules/Pagination";
import RoutListItemPlaceholder from "./RoutListItemPlaceholder";
import RoutList from "./RoutList";
import { useEffect } from "react";
import { observer } from "mobx-react-lite";

export default observer( function RoutDashBoard() {

  const {routStore:{setPagingParams,pagination,loadRouts,loadingNext,setLoadingNext,loadingRouts,routRegistry}} = useStore();
  const {areaStore:{selectedArea}} = useStore();
  const {id} = useParams();


  useEffect(() => {
    if(id) loadRouts(id);
    else loadRouts(selectedArea!.id)
    setPagingParams(new PagingParams(0));
    
}, [ loadRouts]);
  console.log(routRegistry)
  function handleGetNext(){
    setLoadingNext(true);
    setPagingParams(new PagingParams(pagination!.currentPage + 1));
    if(id) loadRouts(id!).then(()=> setLoadingNext(false));
    else loadRouts(selectedArea!.id).then(()=> setLoadingNext(false));
  }
  console.log(loadingRouts);
  console.log('pagination total page: '+pagination?.totalPages)
  return (
    <Grid style={{marginTop:'10em'}}>
      <Grid.Column width='10'>
        {loadingRouts && !loadingNext ?(
          <>
            <RoutListItemPlaceholder />
            <RoutListItemPlaceholder />
          </>
        ):(
          <InfiniteScroll
          pageStart={0}
          loadMore={handleGetNext}
          hasMore={!loadingNext && !!pagination && pagination.currentPage < pagination.totalPages}
          initialLoad={false}
        >
          <RoutList />
        </InfiniteScroll>
        )

        }
        
      </Grid.Column>
      <Grid.Column width={10}>
        <Loader active={loadingNext} />
      </Grid.Column>
    </Grid>
  )
})
