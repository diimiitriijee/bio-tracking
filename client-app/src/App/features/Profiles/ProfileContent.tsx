import { Tab } from "semantic-ui-react"
import ProfilePhotos from "./ProfilePhotos"
import { Profil } from "../../modules/Profil";
import { observer } from "mobx-react-lite";
import ProfileTours from "./ProfileTours";
import ProfileSettings from "./ProfileSettings";
import { useStore } from "../../stores/store";
import ProfileInfo from "./ProfileInfo";



interface Props{
  profile:Profil;
}

export default observer( function ProfileContent({profile}:Props) {
    const { userStore: { user } } = useStore();

    const panes=[
      {menuItem: 'Informacije', render:()=> <ProfileInfo profile={profile} />},
      {menuItem: 'Slike', render:()=> <ProfilePhotos profile={profile} />},
      {menuItem: 'Obilasci', render:()=> <ProfileTours />}
    ]
    const panesSettings=[
      {menuItem: 'Informacije', render:()=> <ProfileInfo profile={profile} />},
      {menuItem: 'Slike', render:()=> <ProfilePhotos profile={profile} />},
      {menuItem: 'Obilasci', render:()=> <ProfileTours />},
      {menuItem: 'Podesavanja naloga', render:()=> <ProfileSettings />},
    ]

  return (
    <Tab
        menu={{fluid:true,vertical:true}}
        menuPosition='right'
        panes= {profile.username == user?.userName? panesSettings : panes}
    />
  )
})
