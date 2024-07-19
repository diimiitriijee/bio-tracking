import './App.css'
import { observer } from 'mobx-react-lite'
import { Outlet, ScrollRestoration } from 'react-router-dom'
import NavBar from '../features/NavBar/NavBar'
import { Container } from 'semantic-ui-react'
import { ToastContainer } from 'react-toastify'
import { useStore } from '../stores/store'
import { useEffect } from 'react'
import LoadingComponent from './LoadingComponent'
import ModalContainer from '../common/modals/ModalContainer'


function App() {

  const {commonStore, userStore} = useStore();

  useEffect(()=>{
    if(commonStore.token){
      userStore.getUser().finally(() => commonStore.setAppLoaded())
    }else{
      commonStore.setAppLoaded();
    }
  }, [commonStore, userStore])

  if(!commonStore.appLoaded) return <LoadingComponent content='Učitavanje aplikacije...' />
  
  return (
    <>
    <ScrollRestoration />
    <ModalContainer />
    <ToastContainer position='bottom-right' hideProgressBar theme='colored' />
    <NavBar />
    <Container style={{marginTop:'10em'}}>
          <Outlet />

    </Container>
    </>
  )
}

export default observer(App)
