import { Grid } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';
import UserList from './UserList';
import { useEffect } from 'react';
import { useStore } from '../../../stores/store';
import LoadingComponent from '../../../layout/LoadingComponent';

export default observer(function AdminDashboard() {

    const {adminStore:{loadUsers,loadingInitial}} = useStore();

    useEffect(() => {
        loadUsers();
    }, [loadUsers]);
    if(loadingInitial) return <LoadingComponent content='Učitavanje korisnika...' />
    return (
        <Grid style={{ marginTop: '10em' }}>
            <Grid.Column width='10'>
                <UserList />
            </Grid.Column>
            <Grid.Column width='6'>
                {/* Placeholder component, stavi ovo u zagradicete */}
            </Grid.Column>
        </Grid>
    );
});
