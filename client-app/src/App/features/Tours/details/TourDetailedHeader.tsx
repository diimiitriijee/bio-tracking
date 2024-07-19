import { observer } from 'mobx-react-lite';
import { Button, Header, Item, Segment, Label, Modal, Form, Rating } from 'semantic-ui-react';
import { Obilazak } from '../../../modules/Obilazak';
import { Link, useNavigate } from 'react-router-dom';
import { format } from 'date-fns';
import { useStore } from '../../../stores/store';
import { useState } from 'react';
import Map from '../../Map/Map';
import TourDetailsPlaceHolder from './TourDetailsPlaceHolder';

interface Props {
    tour: Obilazak;
}

export default observer(function TourDetailedHeader({ tour }: Props) {
    const { tourStore: { updateAttendance, loading, cancleTour, handleRateGuide, loadingInitial } } = useStore();
    const { areaStore: { selectedArea } } = useStore();
    const [rating, setRating] = useState(0);
    const [comment, setComment] = useState("");
    const [modalOpen, setModalOpen] = useState(false);
    const { userStore: { isLoggedIn } } = useStore();
    const navigate = useNavigate();

    console.log(tour);

    const handleSubmitRating = () => {
        if (tour && tour.id) {
            handleRateGuide(tour.id, rating, comment);
            setModalOpen(false);
        }
    };

    if (loadingInitial || !tour) return <TourDetailsPlaceHolder />;

    return (
        <Segment.Group>
            <Segment basic attached='top' style={{ padding: '0' }}>
                {tour.isCancelled && (
                    <Label
                        style={{ position: 'absolute', zIndex: 1000, left: -14, top: 20 }}
                        ribbon
                        color='red'
                        content='Obilazak je otkazan'
                    />
                )}
                <Map />
                <Segment basic>
                    <Item.Group>
                        <Item>
                            <Item.Content>
                                <Header size='huge' content={tour.naziv} style={{ color: 'black' }} />
                                <p>{format(tour.datumOdrzavanja!, 'dd MMM yyyy')}</p>
                                <p>
                                    Vodic{' '}
                                    <strong>
                                        <Link to={`/profiles/${tour.vodic?.username}`}>
                                            {tour.vodic?.ime} {tour.vodic?.prezime}
                                        </Link>
                                    </strong>
                                </p>
                            </Item.Content>
                        </Item>
                    </Item.Group>
                </Segment>
            </Segment>
            <Segment clearing attached='bottom'>
                {isLoggedIn && tour.isVodic ? (
                    <>
                        <Button
                            color={'red'}
                            floated='left'
                            basic
                            content={'Otkazi obilazak'}
                            onClick={() => tour.id && cancleTour(tour.id).then(()=> navigate(`/tours/${selectedArea?.id}`))}
                            loading={loadingInitial}
                            
                        />
                    </>
                ) : isLoggedIn && tour.ide ? (
                    <Button loading={loading} onClick={updateAttendance}>Odjavi se sa obilaska</Button>
                ) : isLoggedIn && (
                    <Button
                        disabled={tour.brojMaxPolaznika === tour.ucesnici.length}
                        loading={loading}
                        onClick={updateAttendance}
                        color='teal'
                        content={tour.brojMaxPolaznika === tour.ucesnici.length ? 'Sva mesta su popunjena' : 'Prijavi se za obilazak'}
                    ></Button>
                )}
                {!tour.isVodic && isLoggedIn && (
                    <Button color='blue' floated='right' onClick={() => setModalOpen(true)}>
                        Ocenite vodica
                    </Button>
                )}
                <Button color='teal' floated='right' as={Link} to={`/routDetails/${tour.ruta?.id}`}>
                    Pregled rute
                </Button>
            </Segment>
            <Modal open={modalOpen} onClose={() => setModalOpen(false)} size='small'>
                <Header content='Ocenite vodica' />
                <Modal.Content>
                    <Form>
                        <Form.Field>
                            <label>Ocena</label>
                            <Rating
                                icon='star'
                                defaultRating={0}
                                maxRating={5}
                                onRate={(_e, { rating }) => setRating(rating as number)}
                            />
                        </Form.Field>
                        <Form.Field>
                            <label>Komentar</label>
                            <textarea
                                placeholder='Vas komentar..'
                                value={comment}
                                onChange={(e) => setComment(e.target.value)}
                            />
                        </Form.Field>
                    </Form>
                </Modal.Content>
                <Modal.Actions>
                    <Button onClick={() => setModalOpen(false)}>Ponisti</Button>
                    <Button color='green' onClick={handleSubmitRating}>
                        Oceni
                    </Button>
                </Modal.Actions>
            </Modal>
        </Segment.Group>
    );
});
