import { Button, Divider, Grid, Header, Item, Rating, Reveal, Segment, Statistic } from "semantic-ui-react";
import defaultUserImage from '../../../assets/Images/user.png';
import { Profil } from "../../modules/Profil";
import { observer } from "mobx-react-lite";
import FollowButton from "./FollowButton";

interface Props{
    profile:Profil;
}

export default observer(function ProfileHeader({profile}:Props) {
  
  return (
    <Segment>
      <Grid>
        <Grid.Column width={8}>
          <Item.Group>
            <Item>
              <Item.Image avatar size='small' src={ profile.slikaProfila || defaultUserImage} />
              <Item.Content verticalAlign='middle'>
                <Header as='h1'>{profile.ime} {profile.prezime}</Header>
              </Item.Content>
            </Item>
          </Item.Group>
        </Grid.Column>
        
        <Grid.Column width={8} textAlign='right'>
          {(profile.brojOdrzanihObilazaka) ? (
            <>
              <Statistic.Group widths={3}>
                <Statistic label='Organizovani obilasci' value={profile.brojOdrzanihObilazaka} />
                <Statistic label='Prijavljeni obilasci' value={profile.brojPrijavljenihObilazaka} />
                <Statistic label='Broj pratilaca' value={profile.followersCount} />
                <Statistic>
                    <Statistic.Value>
                        <Rating icon='star' defaultRating={profile.prosecnaOcena} maxRating={5} disabled />
                    </Statistic.Value>
                    <Statistic.Label>Prosecna ocena</Statistic.Label>
                </Statistic>
              </Statistic.Group>
              <Divider/>
              <FollowButton profile={profile} />
            </>
          ) : (
            <>
              <Statistic.Group widths={1} floated='right'>
                <Statistic label='Prijavljeni obilasci' value={profile.brojPrijavljenihObilazaka} />
              </Statistic.Group>
              <Divider/>
              <Reveal animated='move'>
                
                <Reveal.Content hidden style={{width:'100%'}}>
                  <Button fluid color={true ? 'red' : 'green'} content={true ? 'Odjavi se' : 'Prijavi se'} />
                </Reveal.Content>
              </Reveal>
            </>
          )}
        </Grid.Column>
      </Grid>
    </Segment>
  );
});
