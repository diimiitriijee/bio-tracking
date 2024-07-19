import { Segment, List, Item, Image } from 'semantic-ui-react'
import { Link } from 'react-router-dom'
import { observer } from 'mobx-react-lite'
import defaultUserImage from '../../../../assets/Images/user.png'
import { Profil } from '../../../modules/Profil'


interface Props{
    attendees:Profil[],
}

export default observer(function TourDetailedSidebar ({attendees}:Props) {
    return (
        <>
            <Segment
                textAlign='center'
                style={{ border: 'none' }}
                attached='top'
                secondary
                inverted
                color='teal'
            >
                {attendees.length} {attendees.length <= 1 ? 'Osoba' : 'Osobe' } su se prijavile
            </Segment>
            <Segment attached>
                <List relaxed divided>
                    {
                        attendees.map(attendee => (
                            <Item style={{ position: 'relative' }} key={attendee.username}>
                                <Image size='mini' src={attendee.slikaProfila || defaultUserImage} />
                                <Item.Content verticalAlign='middle'>
                                <Item.Header as='h3'>
                                    <Link to={`/profiles/${attendee.username}`}>{attendee.ime} {attendee.prezime}</Link>
                                </Item.Header>
                                
                                </Item.Content>
                            </Item>
                        ))
                    }
                    

                </List>
            </Segment>
        </>

    )
})

