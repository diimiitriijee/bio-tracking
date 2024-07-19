import { Grid } from "semantic-ui-react";
import GuideList from "./GuideList";
import { useEffect } from "react";
import { useStore } from "../../../stores/store";
import { observer } from "mobx-react-lite";
import LoadingComponent from "../../../layout/LoadingComponent";

export default observer( function GuideDashBoard() {
    const {adminStore:{loadGuideRequests,loadingInitial,loading}} = useStore();
    useEffect(() => {
        loadGuideRequests();
    }, [loadGuideRequests]);
    if(loadingInitial || loading) return <LoadingComponent content="Ucitavanje zahteva vodica..." />

  return (
    <Grid style={{ marginTop: '10em' }}>
            <Grid.Column width='10'>
                <GuideList />
            </Grid.Column>
            <Grid.Column width='6'>
                {/* Placeholder component, stavi ovo u zagradicete */}
            </Grid.Column>
        </Grid>
  )
})
