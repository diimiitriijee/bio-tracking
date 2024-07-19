import { Grid } from "semantic-ui-react";
import ProfileHeader from "./ProfileHeader";
import ProfileContent from "./ProfileContent";
import { observer } from "mobx-react-lite";
import { useParams } from "react-router-dom";
import { useStore } from "../../stores/store";
import { useEffect } from "react";
import LoadingComponent from "../../layout/LoadingComponent";

export default observer( function ProfilePage() {

  const {username} = useParams<{username:string}>();
  const {profileStore} = useStore();
  const {loadingProfile, loadProfile, loadVodicProfile, profile} = profileStore;



  useEffect(() => {
    console.log("Username:", username);
    if (username) {//namerno ovako da mi uvek daje false dok ne smislim uslov, testirao sam sa true i dobro ucita profil vodica ALI KAKO DA ZNAM DA TO POOVEM UNAPRED
      console.log("Username:", username);
      loadProfile(username).then(() => {
        console.log("Profile loaded!");
        
      }).catch(error => {
        console.error("Error loading profile:", error);
      });
    }
    else if(username) {
      loadVodicProfile(username).then(() => {
        console.log("Profile loaded");
        
      }).catch(error => {
        console.error("Error loading profile:", error);
      });
    }
  }, [loadProfile, loadVodicProfile, username]);
  
  if(loadingProfile) return <LoadingComponent content='Ucitavanje profila...' /> 

  return (
    <Grid>
      <Grid.Column width={16}>
        {profile &&
        <>
          <ProfileHeader profile={profile} />
          <ProfileContent profile={profile}/>
        </>
        
        }
      </Grid.Column>
    </Grid>
  )
})
