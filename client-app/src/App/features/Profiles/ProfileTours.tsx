import { SyntheticEvent, useEffect } from "react";
import { useStore } from "../../stores/store";
import { Card, Grid, Header, Tab, TabProps } from "semantic-ui-react";
import { ObilazakKorisnika } from "../../modules/Profil";
import { Link } from "react-router-dom";
import { format } from "date-fns";
import { observer } from "mobx-react-lite";


export default observer( function ProfileTours() {

    const {profileStore} = useStore();
    const {loadUserTours, profile, loadingTours, userTours} = profileStore;
    
    useEffect(()=>{
        loadUserTours(profile!.username);
    },[loadUserTours,profile]);

    
const panes = (profile?.brojOdrzanihObilazaka != null && profile.brojOdrzanihObilazaka > 0) ? 
    [
    {menuItem :'Buduci obilasci', pane:{key:'future'}},
    {menuItem :'Prosli obilasci', pane:{key:'past'}},
    {menuItem :'Vodim obilaske', pane:{key:'hosting'}}
    ]
    :
    [
    {menuItem :'Buduci obilasci', pane:{key:'future'}},
    {menuItem :'Prosli obilasci', pane:{key:'past'}}
    ]



    //ovde se ozvlaci pane na koji smo kliknuli i salje se zahtev za odredjene obilaske
    //SyntheticEvent zato sto nas vise interesuje data
    const handleTabChange = (_e: SyntheticEvent, data: TabProps) => {
        console.log(profile!.username)
        loadUserTours(profile!.username, panes[data.activeIndex as number].pane.key)
        console.log(panes);
    }
  return (
    <Tab.Pane loading={loadingTours}>
        <Grid>
            <Grid.Column width={16}>
                <Header floated="left" icon='calendar' content={'Obilasci'} />
            </Grid.Column>
            <Grid.Column width={16}>
                <Tab 
                    panes={panes}
                    manu={{secondary: true, pointing:true}}
                    onTabChange={(e,data) => handleTabChange(e,data)}
                />
                <br />
                    <Card.Group itemsPerRow={4}>
                        {userTours.map((tour: ObilazakKorisnika) => (
                            <Card
                                as={Link}
                                to={`/tour/${tour.id}`}
                                key={tour.id}
                            >
                                
                                <Card.Content>
                                    <Card.Header textAlign='center'>{tour.naziv}</Card.Header>
                                    <Card.Meta textAlign='center'>
                                        <div>{format(new Date(tour.datumOdrzavanja), 'do LLL')}</div>
                                        <div>{format(new Date(tour.datumOdrzavanja), 'h:mm a')}</div>
                                    </Card.Meta>
                                </Card.Content>
                            </Card>
                        ))}
                    </Card.Group>
            </Grid.Column>
        </Grid>
    </Tab.Pane>
  )
})
