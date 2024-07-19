import { Button, Icon, Item, Label, Segment } from "semantic-ui-react";
import { Obilazak } from "../../../modules/Obilazak";
import { Link } from "react-router-dom";
//import { SyntheticEvent, useState } from "react";
//import { useStore } from "../../../stores/store";
import { format } from "date-fns";
import TourListItemAttendee from "./TourListItemAttendee";
import { observer } from "mobx-react-lite";
import defaultUserImage from '../../../../assets/Images/user.png';

interface Props{
    tour:Obilazak;
}

export default observer( function TourListItem({tour}:Props) {

    //const [target, setTarget] = useState("");
    //const {tourStore} = useStore();
    //const {deleteTour,loading} = tourStore;

    // function handleTourDelete(e:SyntheticEvent<HTMLButtonElement>, id:string){
    //     setTarget(e.currentTarget.name);
    //     deleteTour(id)
    // }
    
  return (

    <Segment.Group>
        <Segment>
            {tour.isCancelled &&
                <Label attached='top' color='red' content='Otkazan obilazak' style={{textAlign:'center'}}  />
            }
            <Item.Group>
                <Item>
                    <Item.Image style={{marginBottom: 5}} size='tiny' circular src={tour.vodic.slikaProfila || defaultUserImage}/>
                    <Item.Content>
                        <Item.Header as={Link} to={`/tours/${tour.id}`}>
                            {tour.naziv}
                        </Item.Header>
                        <Item.Description>Vodic: <Link to={`/profiles/${tour.vodic?.username}`}>{tour.vodic?.ime} {tour.vodic?.prezime}</Link></Item.Description>
                        {
                            tour.isVodic && (
                                <Item.Description>
                                    <Label basic color="red">Ti si vodic</Label>
                                </Item.Description>
                            )
                        }
                        {
                            tour.ide && !tour.isVodic && (
                                <Item.Description>
                                    <Label basic color="green">Prijavio si se da ides</Label>
                                </Item.Description>
                            )
                        }
                    </Item.Content>
                </Item>
            </Item.Group>
        </Segment>
        <Segment>
            <span>
                <Icon name='clock' /> {format(tour.datumOdrzavanja!, 'dd MMM yyyy h:mm aa')}
                <Icon name='marker' /> {tour.mestoOkupljanja}
            </span>
        </Segment>
        <Segment secondary>
            <TourListItemAttendee attendees={tour.ucesnici!} />
        </Segment>
        <Segment clearing>
            <span>{tour.opis}</span>
            <Button 
                as={Link}
                to={`/tour/${tour.id}`}
                color='teal'
                floated='right'
                content='Detalji'
            />
        </Segment>
            
    </Segment.Group>
  )
            
}
)