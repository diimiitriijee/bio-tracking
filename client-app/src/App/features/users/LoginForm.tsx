import { ErrorMessage, Form, Formik } from 'formik'
import TextInput from '../../common/form/TextInput'
import { Button, Header, Label } from 'semantic-ui-react'
import { useStore } from '../../stores/store'
import { observer } from 'mobx-react-lite'
import RegisterForm from './RegisterForm'


export default observer( function LoginForm() {

  const {userStore, modalStore} = useStore();

  return (
    <Formik 
        initialValues={{email:'', password:'', error:null}}
        onSubmit={(values, {setErrors})=>userStore.login(values).catch((error)=> 
          setErrors({error: error.response.data}))}
    >
        {({handleSubmit, isSubmitting, errors})=>(
            <Form className='ui form' onSubmit={handleSubmit} autoComplete='off'>
                <Header as='h2' content='Prijavi se' color='teal' textAlign='center' />
                <TextInput placeholder='Email' name='email' />
                <TextInput placeholder='Lozinka' name='password' type='password' />
                <ErrorMessage 
                  name='error' render={()=><Label style={{marginBottom:10}} basic color='red' content={errors.error} />} />
                <span style={{color: 'blue', textDecoration:'underline', cursor:'pointer'}} 
                onClick={() => modalStore.openModal(<RegisterForm/>)}>Registruj se</span>
                <Button loading={isSubmitting} positive content='Prijavi se' type='submit' fluid />
            </Form>
        )}
    </Formik>
  )
})
