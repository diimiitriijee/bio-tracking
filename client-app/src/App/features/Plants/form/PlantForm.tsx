import { observer } from 'mobx-react-lite';
import * as Yup from 'yup';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { useStore } from '../../../stores/store';
import { BiljkaFormValues } from '../../../modules/Biljke';
import LoadingComponent from '../../../layout/LoadingComponent';
import { v4 as uuid } from 'uuid';
import { Button, Header, Segment } from 'semantic-ui-react';
import { Formik, Form } from 'formik';
import TextInput from '../../../common/form/TextInput';
import TextArea from '../../../common/form/TextArea';
import DropdownInput from '../../../common/form/DropdownInput';

export default observer(function PlantForm() {
  const { id } = useParams();
  const { plantStore } = useStore();
  const { loadPlant, loadingInitial, createPlant, updatePlant, isLoading } = plantStore;
  const navigate = useNavigate();
  const [plant, setPlant] = useState<BiljkaFormValues>(new BiljkaFormValues());

  const validationSchema = Yup.object({
    naziv: Yup.string().required('Polje naziv je obavezano'),
    opis: Yup.string().required('Polje opis je obavezano'),
    vrsta: Yup.string().required('Polje vrsta je obavezno'),
    lekovita: Yup.boolean().required('Polje lekovita je obavezno'),
  });

  useEffect(() => {
    if (id) loadPlant(id).then(plant => setPlant(plant!));
  }, [id, loadPlant]);

  if (loadingInitial) return <LoadingComponent content='Ucitavanje biljke...' />

  function handleFormSubmit(plant: BiljkaFormValues, file: File | null) {
    const formData = new FormData();
    formData.append('naziv', plant.naziv);
    formData.append('opis', plant.opis);
    formData.append('vrsta', plant.vrsta);
    formData.append('lekovita', plant.lekovita.toString());
    if (file) {
      formData.append('slika', file);
    }

    if (!plant.id) {
      let newPlant = {
        ...plant,
        id: uuid()
      };
      createPlant(formData, newPlant).then(() => navigate('/'));
    } else {
      updatePlant(plant).then(() => navigate('/'));
    }
  }

  return (
    <Segment clearing>
      <Header content='Podaci o biljci' sub color='teal' />
      <Formik
        validationSchema={validationSchema}
        enableReinitialize
        initialValues={plant}
        onSubmit={(values, { setSubmitting }) => {
          const fileInput = document.querySelector<HTMLInputElement>('input[name="slika"]');
          const file = fileInput?.files ? fileInput.files[0] : null;
          handleFormSubmit(values, file);
          setSubmitting(false);
        }}
      >
        {({ handleSubmit, isValid, isSubmitting, dirty }) => (
          <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
            <TextInput name='naziv' placeholder='Naziv' />
            <TextArea rows={3} placeholder='Opis' name='opis' />
            <Header content='Specifikacija biljke' sub color='teal' />
            <TextInput placeholder='Vrsta' name='vrsta' />
            <DropdownInput placeholder='Izaberite tip' name='lekovita' label='Tip biljke' />
            <input
              type="file"
              name="slika"
            />
            <Header sub color='teal' />

            <Button
              disabled={isSubmitting || !dirty || !isValid}
              loading={isLoading}
              floated='right'
              positive
              type='submit'
              content='Dodaj'
            />
            <Button as={Link} to={'/'} floated='right' type='button' content='Nazad' />
          </Form>
        )}
      </Formik>
    </Segment>
  );
});
