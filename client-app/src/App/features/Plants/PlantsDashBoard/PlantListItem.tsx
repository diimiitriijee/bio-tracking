import { useState } from "react";
import { useStore } from "../../../stores/store";
import { Button, Item, Segment } from "semantic-ui-react";
import { Biljka } from "../../../modules/Biljke";
import { Link } from "react-router-dom";
import { observer } from "mobx-react-lite";

interface Props{
    plant:Biljka;
}

export default observer( function PlantListItem({plant}:Props) {
    const [] = useState("");
    const {areaStore:{deletePlantFromArea, isLoading}} = useStore();
    const { userStore: { isLoggedIn, isVodic } } = useStore();

    /*const {deleteplant,loading} = plantStore;

    function handleplantDelete(e:SyntheticEvent<HTMLButtonElement>, id:string){
        setTarget(e.currentTarget.name);
        deleteplant(id)
    }*/
    
  return (

    <Segment.Group>
        <Segment>
            
            <Item.Group>
                <Item>
                    <Item.Image size='tiny' circular src={plant.slika}/>
                    <Item.Content>
                        <Item.Header as={Link} to={`/plants/${plant.id}`}>
                            {plant.naziv}
                        </Item.Header>
                        
                    </Item.Content>
                </Item>
            </Item.Group>
        </Segment>
        <Segment>
        <Item.Description content={plant.opis} /> 

        </Segment>
        <Segment clearing>
            {isLoggedIn && isVodic && (
                <Button 
                loading={isLoading}
                onClick={()=>deletePlantFromArea(plant.id)}
                color='red'
                floated='right'
                content='Obrisi biljku'
            />
            )}
            
            <Button 
                as={Link}
                to={`/plantDetails/${plant.id}`}
                color='teal'
                floated='right'
                content='Detalji'
            />
        </Segment>
            
    </Segment.Group>
  )
            
})
