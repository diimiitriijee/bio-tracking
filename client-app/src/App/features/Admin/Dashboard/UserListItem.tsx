import { Segment, Item, Button } from 'semantic-ui-react';
import { UserForAdmin } from '../../../modules/User';
import defaultUserImage from '../../../../assets/Images/user.png'
import { Link } from 'react-router-dom';
import { useStore } from '../../../stores/store';

interface Props {
    user: UserForAdmin;
}

export default function UserListItem({ user }: Props) {
    const { adminStore: { banUser }} = useStore();

    const handleBan = (userId : string) => {
        banUser(userId);
    };
    return (
        <Segment.Group>
            <Segment>
                <Item.Group>
                    <Item>
                        <Item.Image style={{marginBottom: 5}} size='tiny' circular src={user.slika || defaultUserImage}/>
                        <Item.Content>
                            <Item.Header as={Link} to={`/profiles/${user.userName}`}>
                                {user.userName}
                            </Item.Header>
                            <Item.Description>{user.ime} {user.prezime}</Item.Description>
                        </Item.Content>
                    </Item>
                </Item.Group>
            </Segment>
            <Segment>
                <Item.Group>
                    <Item>
                        <Item.Content>
                            <Item.Header>
                                Role
                            </Item.Header>
                            {
                                user.roles.map((role)=>(
                                    <Item.Description key={role.id}>{role.name}</Item.Description>
                                ))
                            }
                            <Button 
                                as={Link}
                                to={''}
                                color='red'
                                floated='right'
                                content='Banuj korisnika'
                                onClick={()=>handleBan(user.id)}
                            />
                        </Item.Content>
                    </Item>
                </Item.Group>
            </Segment>
        </Segment.Group>
    );
}
