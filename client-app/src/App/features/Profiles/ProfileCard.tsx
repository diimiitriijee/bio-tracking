import { observer } from "mobx-react-lite";
import { Profil } from "../../modules/Profil"
import { Card , Image} from "semantic-ui-react";
import { Link } from "react-router-dom";
import defaultUserImage from '../../../assets/Images/user.png' ;


interface Props{
    profile:Profil;
}

export default observer( function ProfileCard({profile}:Props) {
  return (
    <Card as={Link} to={`/profiles/${profile.username}`}>
        <Image src={profile.slikaProfila || defaultUserImage} />
        <Card.Content>
            <Card.Header>{profile.ime} {profile.prezime}</Card.Header>
            <Card.Description>{profile.telefon}</Card.Description>
        </Card.Content>
        <Card.Content extra>
            kude je ovo
        </Card.Content>
    </Card>
  )
})
