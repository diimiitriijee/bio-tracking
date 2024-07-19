import { Header, List } from 'semantic-ui-react';
import { observer } from 'mobx-react-lite';
import UserListItem from './UserListItem';
import { Fragment } from 'react';
import { useStore } from '../../../stores/store';


export default observer(function UserList() {
    const { adminStore } = useStore();
    const { users,  } = adminStore;

    
    console.log(users)
    
    return (
        <Fragment>
            <Header sub color='teal'>Korisnici</Header>
            <List>
                {users.map(user => (
                    <UserListItem key={user.userName} user={user} />
                ))}
            </List>
        </Fragment>
    );
});
