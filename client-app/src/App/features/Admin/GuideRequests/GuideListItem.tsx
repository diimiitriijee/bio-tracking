import { observer } from "mobx-react-lite";
import { VodicZahtev } from "../../../modules/Vodic"
import { Button, Item, Modal, Segment, Image } from "semantic-ui-react";
import defaultUserImage from '../../../../assets/Images/user.png'
import { useStore } from "../../../stores/store";
import { useState } from "react";


interface Props{
    guide: VodicZahtev;
}

export default observer( function GuideListItem({guide}:Props) {

    const {adminStore:{postGuide, loading}} = useStore();
    const [open, setOpen] = useState(false);
  return (
    <Segment.Group>
        <Segment>
            
            <Item.Group>
                <Item>
               
            <Item.Image 
                style={{marginBottom: 5,cursor: 'pointer'}}  
                circular 
                src={guide.putanjaSlikeDiplome || defaultUserImage} 
                onClick={() => setOpen(true)} 
            />
            <Modal
                open={open}
                onClose={() => setOpen(false)}
                size='large'
                dimmer='blurring'
            >
                <Modal.Content>
                    <Image src={guide.putanjaSlikeDiplome || defaultUserImage} fluid />
                </Modal.Content>
            </Modal>
                           <Item.Content>
                        <Item.Header>
                            {guide.ime} {guide.prezime}
                        </Item.Header>
                        <Item.Description>{guide.email}</Item.Description>
                    </Item.Content>
                </Item>
            </Item.Group>
        </Segment>
        <Segment>
            <Item.Group>
                <Item>
                    <Item.Content>
                        
            <Button 
                onClick={()=>postGuide(guide.id)}
                loading={loading}
                color='green'
                floated='right'
                content='Prihvati vodica'
            />
                    </Item.Content>
                    </Item>
            </Item.Group>
            </Segment>
        </Segment.Group>
  )
})
