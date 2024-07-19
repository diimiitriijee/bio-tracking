import { useEffect, useState } from 'react'
import Map from '../../Map/Map'
import { useStore } from '../../../stores/store'
import * as Yup from 'yup';
import { Podrucje, PodrucjeFormValue } from '../../../modules/Podrucje';
import {v4 as uuid} from "uuid";
import { Form, Link, useNavigate } from 'react-router-dom';
import { Button, Header, Segment } from 'semantic-ui-react';
import { Formik } from 'formik';
import TextInput from '../../../common/form/TextInput';
import { toast } from 'react-toastify';


export default function AreaForm() {

    const {areaStore} = useStore();
    const {setIsCreatingArea, getNewCoords,createArea, setNewCoords, isLoading} = areaStore;
    const navigate = useNavigate();
    const [area] = useState<PodrucjeFormValue>(new PodrucjeFormValue());

    useEffect(()=>{
        setIsCreatingArea(true);
    }, [setIsCreatingArea]);

    const validationSchema = Yup.object({
        oblast: Yup.string().required('Polje oblast je obavezno')
    })

    function hanldeFormSubmit(area:Podrucje){

        if(getNewCoords.length >= 3)
            {
                let newArea = new PodrucjeFormValue(area); 
                newArea.koordinate = getNewCoords;
                newArea.id = uuid();
                createArea(newArea).then(()=>navigate('/'))
            }
        else{
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
        initialValues={area}
        onSubmit={values=> hanldeFormSubmit(values)}>
        {({handleSubmit, isValid, dirty})=>(
          <Form className='ui form' onSubmit={handleSubmit} autoComplete='off' >
            <Map />
            <TextInput name='oblast' placeholder='Oblast' />
            <Button 
              disabled={isLoading || !dirty || !isValid}
              loading={isLoading}
              floated='right' 
              positive type='submit' content='Dodaj'/>
            <Button 
                as={Link} 
                to={'/'} 
                floated = 'right' 
                type = 'button' 
                content = 'Nazad'
                onClick={() => {setNewCoords(undefined); setIsCreatingArea(false);} }  />
        </Form>
        )}
      </Formik>
        
    </Segment>
    </>

  )
}
