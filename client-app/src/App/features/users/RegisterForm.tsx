import { ErrorMessage, Form, Formik } from 'formik';
import { useState } from 'react';
import TextInput from '../../common/form/TextInput';
import DateInput from '../../common/form/DateInput';
import { Button, Header, Dropdown } from 'semantic-ui-react';
import { useStore } from '../../stores/store';
import { observer } from 'mobx-react-lite';
import * as Yup from 'yup';
import ValidationError from '../Errors/ValidationError';
import 'react-datepicker/dist/react-datepicker.css';

export default observer(function RegisterForm() {
  const { userStore } = useStore();
  const [registrationType, setRegistrationType] = useState('user');

  const registrationTypeOptions = [
    { key: 'user', text: 'User', value: 'user' },
    { key: 'vodic', text: 'Vodic', value: 'vodic' }
  ];

  return (
    <Formik
      initialValues={{
        email: '',
        password: '',
        ime: '',
        prezime: '',
        username: '',
        telefon: '',
        datumRodjenja: new Date(),
        strucnaSprema: '',
        slikaDiplome: '',
        error: null,
      }}
      validationSchema={Yup.object({
        ime: Yup.string().required('Ime je obavezno'),
        prezime: Yup.string().required('Prezime je obavezno'),
        username: Yup.string().required('Korisničko ime je obavezno'),
        email: Yup.string().email('Neispravan email').required('Email je obavezan'),
        telefon: Yup.string().required('Telefon je obavezan'),
        datumRodjenja: Yup.date().required('Datum rođenja je obavezan').nullable(),
        ...(registrationType === 'user' && {
          password: Yup.string().required('Lozinka je obavezna'),
        }),
        ...(registrationType === 'vodic' && {
          strucnaSprema: Yup.string().required('Stručna sprema je obavezna'),
          slikaDiplome: Yup.mixed().required('Slika diplome je obavezna'),
        }),
      })}
      onSubmit={(values, { setErrors }) => {
        if (registrationType === 'user') {
          userStore.register(values)
            .catch((error) => setErrors(error.response.data.errors));
        } else {
          const formData = new FormData();
          formData.append('email', values.email);
          formData.append('ime', values.ime);
          formData.append('prezime', values.prezime);
          formData.append('username', values.username);
          formData.append('telefon', values.telefon);
          if (values.datumRodjenja) {
            formData.append('datumRodjenja', new Date(values.datumRodjenja).toISOString());
          }
          formData.append('strucnaSprema', values.strucnaSprema);
          if (values.slikaDiplome) {
            formData.append('slikaDiplome', values.slikaDiplome);
          }
          userStore.registerVodic(formData)
            .catch((error) => setErrors(error.response.data.errors));
        }
      }}
    >
      {({ handleSubmit, setFieldValue, isSubmitting, errors, isValid, dirty }) => (
        <Form className="ui form error" onSubmit={handleSubmit} autoComplete="off">
          <Header as="h2" content="Register" color="teal" textAlign="center" />
          <Dropdown
            placeholder="Select User Type"
            fluid
            selection
            options={registrationTypeOptions}
            value={registrationType}
            onChange={(_e, { value }) => setRegistrationType(value as string)}
          />
          <TextInput placeholder="Ime" name="ime" />
          <TextInput placeholder="Prezime" name="prezime" />
          <TextInput placeholder="Email" name="email" />
          <TextInput placeholder="Telefon" name="telefon" />
          <TextInput placeholder="Korisnicko ime" name="username" />
          <DateInput 
            placeholderText="Datum rođenja" 
            name="datumRodjenja" 
            showYearDropdown 
            dateFormat="dd/MM/yyyy" 
          />
          {registrationType === 'user' && (
            <TextInput placeholder="Lozinka" name="password" type="password" />
          )}
          {registrationType === 'vodic' && (
            <>
              <TextInput placeholder="Stručna sprema" name="strucnaSprema" />
              <input
                type="file"
                name="slikaDiplome"
                onChange={(event) => setFieldValue("slikaDiplome", event.currentTarget.files![0])}
              />
            </>
          )}
          <ErrorMessage
            name="error"
            render={() => <ValidationError errors={errors.error as unknown as string[]} />}
          />
          <Button
            disabled={!isValid || !dirty || isSubmitting}
            loading={isSubmitting}
            positive
            content="Register"
            type="submit"
            fluid
          />
        </Form>
      )}
    </Formik>
  );
});
