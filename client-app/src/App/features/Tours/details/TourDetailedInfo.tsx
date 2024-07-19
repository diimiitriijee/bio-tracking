import { observer } from 'mobx-react-lite';
import {Segment, Grid, Icon} from 'semantic-ui-react'
import { Obilazak } from '../../../modules/Obilazak';
import { format } from 'date-fns';

interface Props {
    tour: Obilazak
}

export default observer(function ActivityDetailedInfo({tour}: Props) {
    return (
        <Segment.Group>
            <Segment attached='top'>
                <Grid>
                    <Grid.Column width={1}>
                        <Icon size='large' color='teal' name='info'/>
                    </Grid.Column>
                    <Grid.Column width={15}>
                        <p>{tour.opis}</p>
                    </Grid.Column>
                </Grid>
            </Segment>
            <Segment attached>
                <Grid verticalAlign='middle'>
                    <Grid.Column width={1}>
                        <Icon name='calendar' size='large' color='teal'/>
                    </Grid.Column>
                    <Grid.Column width={15}>
            <span>
              {format(tour.datumOdrzavanja!, 'dd MMM yyyy h:mm aa')}
            </span>
                    </Grid.Column>
                </Grid>
            </Segment>
            <Segment attached>
                <Grid verticalAlign='middle'>
                    <Grid.Column width={1}>
                        <Icon name='marker' size='large' color='teal'/>
                    </Grid.Column>
                    <Grid.Column width={11}>
                        <span>{tour.mestoOkupljanja}</span>
                    </Grid.Column>
                </Grid>
            </Segment>
        </Segment.Group>
    )
})
