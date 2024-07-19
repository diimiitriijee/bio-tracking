import {  useEffect, useState } from 'react'
import { Button , Header, Segment } from 'semantic-ui-react'
import { useStore } from '../../../stores/store';
import { observer } from 'mobx-react-lite';
import { Link, useNavigate, useParams } from 'react-router-dom';
import {  ObilazakFromValues } from '../../../modules/Obilazak';
import LoadingComponent from '../../../layout/LoadingComponent';
import {v4 as uuid} from "uuid";
import { Formik, Form} from 'formik';
import * as Yup from 'yup';
import TextInput from '../../../common/form/TextInput';
import TextArea from '../../../common/form/TextArea';
import DateInput from '../../../common/form/DateInput';
import RoutDashBoard from '../../Rout/RoutDashBoard/RoutDashBoard';


export default observer( function TourForm() {

  const {tourStore} = useStore();
  const { createTour, updateTour,  loadingInitial, loadTour} = tourStore;
  const {areaStore:{selectedArea},routStore:{selectedRoutId, setIsTourForm}} = useStore();
  const {id} = useParams();
  const navigate = useNavigate();

  const [tour,setTour] = useState<ObilazakFromValues>(new ObilazakFromValues());

  const validationSchema = Yup.object({
    naziv: Yup.string().required('Naslov je obavezan'),
    opis: Yup.string().required('Opis je obavezan'),
    datumOdrzavanja: Yup.string().required('Datum je obavezan'),
    brojMaxPolaznika: Yup.string().required('Broj polaznika je obavezan'),
    mestoOkupljanja: Yup.string().required('Mesto okupljanja je obavezno')

  })

  useEffect(()=>{
    if(id) loadTour(id).then(tour => setTour(new ObilazakFromValues(tour as ObilazakFromValues)));
    setIsTourForm(true);
    
    return(()=>setIsTourForm(false))
  },[id,loadTour])
  console.log(tour.id)

  if(loadingInitial) return <LoadingComponent content='Loading tour...' />

  function hanldeTourFormSubmit(tour:ObilazakFromValues){
    if(!tour.id){
      let newTour = {
        ...tour,
        id: uuid()
      };  
      
      createTour(selectedArea!.id,selectedRoutId!,newTour,).then(()=>navigate(`/tours/${selectedArea!.id}`))
    }else{
      updateTour(tour).then(()=> navigate(`/tour/${tour.id}`))
    }
    
  }


  
  return (
    <>
    <Segment clearing>
      <Header content='Detalji obilaska' sub color='teal' />
      <Formik 
      validationSchema={validationSchema}
        enableReinitialize 
        initialValues={tour}
        onSubmit={values=> hanldeTourFormSubmit(values)}>
        {({handleSubmit, isValid, isSubmitting, dirty})=>(
          <Form className='ui form' onSubmit={handleSubmit} autoComplete='off' >
            <TextInput name='naziv' placeholder='Naziv' />
            <TextArea rows={3} placeholder ='Opis'  name='opis' ></TextArea>
            <DateInput 
              placeholderText= 'Datum'  
              name='datumOdrzavanja' 
              showTimeSelect
              timeCaption='time'
              dateFormat='MMMM d, yyyy h:mm aa'
              />      
            <TextInput  placeholder ='BrojPolaznika'  name='brojMaxPolaznika' ></TextInput>
            <Header content='Detalji lokacije' sub color='teal' />
            
            <TextInput placeholder ='MestoOkupljanja'  name='mestoOkupljanja' ></TextInput>

            <Button 
              disabled={!selectedRoutId || isSubmitting || !dirty || !isValid}
              loading={isSubmitting}
              floated='right' 
              positive type='submit' content='Dodaj'/>
            <Button as={Link} to={`/tours/${selectedArea!.id}`} floated = 'right' type = 'button' content = 'Nazad'  />
        </Form>
        )}
      </Formik>
        
    </Segment>
      <RoutDashBoard />
    </>
  )
})
