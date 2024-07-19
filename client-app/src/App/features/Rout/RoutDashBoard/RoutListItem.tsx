import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { Button, Icon, Item, Segment } from "semantic-ui-react";
import { Ruta } from "../../../modules/Ruta";
import { useStore } from "../../../stores/store";

interface Props{
    rout:Ruta;
}



export default observer( function RoutListItem({rout}:Props) {
    const {routStore:{setSelectedRoutId, selectedRoutId, isTourForm}} = useStore();
    const isDisabled = selectedRoutId !== undefined && selectedRoutId !== rout.id;
    
  return (
    <Segment.Group>
        <Segment>
            
            <Item.Group>
                <Item>
                    <Item.Content>
                        <Item.Description>Opis: {rout.opis}</Item.Description>
                        
                    </Item.Content>
                </Item>
            </Item.Group>
        </Segment>
        <Segment>
            <span>
                <Icon name='road' /> {rout.duzina}
                
            </span>
        </Segment>
        
        <Segment clearing>
            <Button 
                as={Link}
                to={`/routDetails/${rout.id}`}
                color='teal'
                floated='right'
                content='Detalji'
            />
            {isTourForm && (<Button
                color='green'
                floated='right'
                content='Izaberi'
                onClick={() => setSelectedRoutId(rout.id)}
                disabled={isDisabled}
            />)}
        </Segment>
            
    </Segment.Group>
  )
})
