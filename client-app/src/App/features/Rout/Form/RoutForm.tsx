import { Formik } from "formik";
import { Button, Form, Header, Segment } from "semantic-ui-react";
import TextInput from "../../../common/form/TextInput";
import { Link, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { useStore } from "../../../stores/store";
import * as Yup from 'yup';
import { RutaFormValues } from "../../../modules/Ruta";
import { toast } from "react-toastify";
import {v4 as uuid} from "uuid";
import Map from '../../Map/Map';
import { observer } from "mobx-react-lite";
import './RoutLength.css';
import DropdownInput from '../../../common/form/DropdownInput'; // Importovanje komponente DropdownInput

export default observer( function RoutForm() {
    
    const {routStore, areaStore} = useStore();
    const {createRout, getNewCoords, setNewCoords, setIsCreatingRout, isCreating, routLength} = routStore;
    const {selectedArea} = areaStore;
    const navigate = useNavigate();
    const [rout] = useState<RutaFormValues>(new RutaFormValues());

    const validationSchema = Yup.object({
      opis: Yup.string().required('Opis je obavezan'),
      tip: Yup.string().required('Polje tip je obavezno'),
      zaDecu: Yup.boolean().required('Polje za decu je obavezno'),
      prohodnost: Yup.string().required('Polje prohodnost je obavezno'),
      uspon: Yup.string().required('Polje uspon je obavezno')
    });

    useEffect(()=>{
      setIsCreatingRout(true);
    }, [setIsCreatingRout]);

    function handleFormSubmit(rout:RutaFormValues){
        if(getNewCoords.length >= 2) {
            let newRout ={...rout, id:uuid(), koordinate:getNewCoords, duzina:routLength};
            createRout(newRout, selectedArea!.id).then(() => navigate(`/areaRouts/${selectedArea!.id}`));
        } else {
            toast.error('Molimo vas da kreirate podrucje na mapi sa minimum 3 koordinate.');
        }
    }

    return (
        <>
            <Segment clearing>
                <Header content='Podrucje' sub color='teal' />
                <Formik 
                    validationSchema={validationSchema}
                    enableReinitialize 
                    initialValues={rout}
                    onSubmit={values => handleFormSubmit(values)}
                >
                    {({handleSubmit, isValid, dirty}) => (
                        <Form className='ui form' onSubmit={handleSubmit} autoComplete='off' >
                            <Map />
                            <TextInput name='opis' placeholder='Opis' />
                            <div className="input-container"> 
                                <input
                                    value={routLength}
                                    placeholder='Duzina'
                                    readOnly // Da biste ovo polje bilo samo za prikaz
                                />
                                <span>km</span>
                            </div>
                            <TextInput name='tip' placeholder='Tip' />

                            
                                <DropdownInput name='zaDecu' placeholder='Za Decu' label='Za Decu' />
                            

                            <TextInput name='prohodnost' placeholder='Prohodnost' />
                            <TextInput name='uspon' placeholder='Uspon' />
                            <Button 
                                disabled={isCreating || !dirty || !isValid}
                                loading={isCreating}
                                floated='right' 
                                positive 
                                type='submit' 
                                content='Dodaj'
                            />
                            <Button 
                                as={Link} 
                                to={`/tours/${selectedArea!.id}`} 
                                floated='right' 
                                type='button' 
                                content='Nazad'
                                onClick={() => {setNewCoords(undefined); setIsCreatingRout(false);} }  
                            />
                        </Form>
                    )}
                </Formik>
            </Segment>
        </>
    );
});
