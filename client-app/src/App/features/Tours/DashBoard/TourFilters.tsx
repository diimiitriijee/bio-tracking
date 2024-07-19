import { observer } from "mobx-react-lite";
import Calendar from "react-calendar";
import { Header, Menu } from "semantic-ui-react";
import { useStore } from "../../../stores/store";

export default observer( function TourFilters() {

    const {tourStore:{predicate, setPredicate}} = useStore();
    const { userStore: { isLoggedIn } } = useStore();

  return (
    <>
        <Menu vertical size='large' style={{width:'100%', marginTop: 25}}>
            <Header icon='filter' attached color="teal" content='Filter' />
            <Menu.Item 
                content='Svi predstojeci obilasci'
                active={predicate.has('all')}
                onClick={() => setPredicate('all', 'true')}
            />
            {
                isLoggedIn && 
                <>
                    <Menu.Item 
                        content='Obilasci na koje idem' 
                        active={predicate.has('isGoing')}
                        onClick={() => setPredicate('isGoing', 'true')}
                    />
                    <Menu.Item 
                        content='Obilasci koje vodim' 
                        active={predicate.has('isHost')}
                        onClick={() => setPredicate('isHost', 'true')}
                    />
                </>
            }
            
        </Menu>
        <Header />
        <Calendar 
            onChange={(date) => setPredicate('startDate', date as Date)}
            value={predicate.get('startDate') || new Date()}
        />
    </>
)
})
