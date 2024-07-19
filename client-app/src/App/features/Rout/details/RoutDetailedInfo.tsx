import { Grid, Icon, Segment } from "semantic-ui-react";
import { Ruta } from "../../../modules/Ruta";

interface Props{
    rout:Ruta;
}
export default function RoutDetailedInfo({rout}:Props) {
    
  return (
    <Segment.Group>
            <Segment attached='top'>
                <Grid>
                    <Grid.Column width={1}>
                        <Icon size='large' color='teal' name='road'/>
                    </Grid.Column>
                    <Grid.Column width={15}>
                        <p>{rout.duzina}</p>
                    </Grid.Column>
                </Grid>
            </Segment>
            <Segment attached>
                <Grid verticalAlign='middle'>
                    <Grid.Column width={1}>
                        <Icon name='child' size='large' color='teal'/>
                    </Grid.Column>
                    <Grid.Column width={15}>
                        <span>
                        {rout.zaDecu ? 'Prohodno za decu' : 'Nije prohodno za decu'}
                        </span>
                    </Grid.Column>
                </Grid>
            </Segment>
            <Segment attached>
                <Grid verticalAlign='middle'>
                    <Grid.Column width={1}>
                        <Icon name='tree' size='large' color='teal'/>
                    </Grid.Column>
                    <Grid.Column width={11}>
                        <span>{rout.prohodnost}</span>
                    </Grid.Column>
                </Grid>
            </Segment>
            <Segment attached>
                <Grid verticalAlign='middle'>
                    <Grid.Column width={1}>
                        <Icon name='chart line' size='large' color='teal'/>
                    </Grid.Column>
                    <Grid.Column width={11}>
                        <span>{rout.uspon} m</span>
                    </Grid.Column>
                </Grid>
            </Segment>
        </Segment.Group>
  )
}
