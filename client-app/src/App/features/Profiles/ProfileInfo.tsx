import { Grid, Header, Segment, Tab, Icon, Divider, Card } from "semantic-ui-react";
import { Profil } from "../../modules/Profil";
import { observer } from "mobx-react-lite";
import { format } from "date-fns";

interface Props {
  profile: Profil;
}

export default observer(function ProfileInfo({ profile }: Props) {
  const formattedDatumRodjenja = format(new Date(profile.datumRodjenja!), 'dd.MM.yyyy.');

  return (
    <Tab.Pane>
      <Grid>
        <Grid.Row>
          <Grid.Column width={16}>
            <Header floated='left' icon='info' textAlign='center' content='Informacije'>
            </Header>
          </Grid.Column>
        </Grid.Row>

        <Grid.Row>
          <Grid.Column width={16}>
            <Card fluid>
              <Card.Content>
                <Icon name='calendar' size='large' floated='left' /><br/><br/>
                <Card.Header>Datum rođenja</Card.Header>
                <Card.Meta>{formattedDatumRodjenja}</Card.Meta>
              </Card.Content>
            </Card>

            <Card fluid>
              <Card.Content>
                <Icon name='phone' size='large' floated='left' /><br/><br/>
                <Card.Header >Kontakt telefon</Card.Header>
                <Card.Meta>{profile.telefon}</Card.Meta>
              </Card.Content>
            </Card>
          </Grid.Column>
        </Grid.Row>
      </Grid>
    </Tab.Pane>
  );
});
