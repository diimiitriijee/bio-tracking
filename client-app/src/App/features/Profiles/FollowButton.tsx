import { observer } from "mobx-react-lite";
import { Profil } from "../../modules/Profil"
import { Button, Reveal } from "semantic-ui-react";
import { useStore } from "../../stores/store";
import { SyntheticEvent } from "react";


interface Props{
    profile: Profil;
}

export default observer( function FollowButton({profile}:Props) {

    const {profileStore, userStore} = useStore();
    const {updateFollowing, loading} = profileStore;

    if(userStore.user?.userName === profile.username) return null;
    //zbog card komponente koja je linkovana zalima da kikom na folow ne odemo na pogresnu putanju
    function handleFollow(e:SyntheticEvent, username: string){
        e.preventDefault();
        profile.following ? updateFollowing(username, false) : updateFollowing(username,true);
    }

  return (
    <Reveal animated='move'>
        <Reveal.Content visible style={{width:'100%'}}>
            <Button fluid color='teal' content={profile.following ? 'Pratis' : 'Ne pratis'} />
        </Reveal.Content>
        <Reveal.Content hidden style={{width:'100%'}}>
            <Button 
                fluid 
                color={profile.following ? 'red' : 'green'} 
                content={profile.following ? 'Otprati' : 'Zaprati'}
                loading={loading}
                onClick={(e) => handleFollow(e,profile.username)}
            />
        </Reveal.Content>
        </Reveal>
  )
})
