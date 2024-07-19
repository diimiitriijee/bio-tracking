import { observer } from "mobx-react-lite"
import { List, Image, Popup } from "semantic-ui-react"
import defaultUserImage from '../../../../assets/Images/user.png'
import { Link } from "react-router-dom";
import { Profil } from "../../../modules/Profil";
import ProfileCard from "../../Profiles/ProfileCard";

interface Props{
    attendees: Profil[];
}
export default observer( function TourListItemAttendee({attendees}:Props) {
  return (
    <List horizontal>
        {
            
            attendees?.map(attendee =>(
                <Popup
                    hoverable
                    ker={attendee.username}
                    trigger={
                        <List.Item key={attendee.username} as={Link} to={`/profiles/${attendee.username}`}>
                            <Image size='mini' circular src={attendee.slikaProfila || defaultUserImage} />
                        </List.Item>
                    }
                >
                    <Popup.Content>
                        <ProfileCard profile={attendee} />
                    </Popup.Content>
                </Popup>
                
            ))
        }
        
    </List>
  )
})
