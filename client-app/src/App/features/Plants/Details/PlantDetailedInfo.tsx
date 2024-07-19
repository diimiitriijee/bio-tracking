import { observer } from 'mobx-react-lite'
//import React from 'react'
import { Grid, Icon, Segment } from 'semantic-ui-react'
import { Biljka } from '../../../modules/Biljke'

interface Props{
    plant:Biljka;
}

export default observer( function PlantDetailedInfo({plant}:Props) {
  return (
    <Segment.Group>
            <Segment attached='top'>
                <Grid>
                    <Grid.Column width={1}>
                        <Icon size='large' color='teal' name='info'/>
                    </Grid.Column>
                    <Grid.Column width={15}>
                        <p>{plant.opis}</p>
                    </Grid.Column>
                </Grid>
            </Segment>
        </Segment.Group>
  )
})
