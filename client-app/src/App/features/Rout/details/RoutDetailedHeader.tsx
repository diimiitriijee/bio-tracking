import { Header, Item, Segment } from "semantic-ui-react";
import Map from '../../Map/Map'
import { Ruta } from "../../../modules/Ruta";

interface Props{
    rout:Ruta;
}
export default function RoutDetailedHeader({rout}:Props) {
    
  return (
    <Segment.Group>
            <Segment basic attached='top' style={{ padding: '0' }}>
                
                <Map />
                <Segment basic>
                    <Item.Group>
                        <Item>
                            <Item.Content>
                                <Header
                                    size='huge'
                                    content={rout.opis}
                                    style={{ color: 'black' }}
                                />
                                <p>
                                    Tip: {rout.tip}
                                </p>
                            </Item.Content>
                        </Item>
                    </Item.Group>
                </Segment>
            </Segment>
            
            
        </Segment.Group>
  )
}
