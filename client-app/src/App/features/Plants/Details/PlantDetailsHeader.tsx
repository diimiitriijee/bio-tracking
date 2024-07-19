//import React from 'react'
import { Header, Item, Image, Segment } from 'semantic-ui-react'
import { Biljka } from '../../../modules/Biljke'

interface Prop{
    plant:Biljka;
}

export default function PlantDetailedHeader({plant}:Prop) {
  return (
    <Segment.Group>
            <Segment basic attached='top' style={{padding: '0'}}>
                <Image size='huge' src={plant.slika} />
                <Segment basic>
                    <Item.Group>
                        <Item>
                            <Item.Content>
                                <Header
                                    size='huge'
                                    content={plant.naziv}
                                    
                                />
                                <p>{plant.lekovita?'Lekovita':'Nije lekovita'}</p>
                                <p>
                                    Vrsta: <strong>{plant.vrsta}</strong>
                                </p>
                            </Item.Content>
                        </Item>
                    </Item.Group>
                </Segment>
            </Segment>
            <Segment clearing attached='bottom'>
                
                
            </Segment>
        </Segment.Group>
  )
}
