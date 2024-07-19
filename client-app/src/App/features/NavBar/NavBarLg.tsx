import { Button, Container, Dropdown, Image, Menu } from 'semantic-ui-react'
import "./NavBar.css"
import { Link, NavLink } from 'react-router-dom';
import { useStore } from '../../stores/store';
import { observer } from 'mobx-react-lite';
import LoginForm from '../users/LoginForm';
import defaultUserImage from '../../../assets/Images/user.png'
import UserStore from '../../stores/userStore';



export default observer( function NavBarLg() {

  const {userStore :{isLoggedIn, user, loguot, isAdmin, isVodic},modalStore} = useStore();
  const {tourStore:{setAreaID}} = useStore();

  return (
    <Menu inverted fixed="top">
        <Container>
            <Menu.Item as={NavLink} to='' name='Home'/>
            <Menu.Item as={NavLink} to='/allTours' onClick={()=>setAreaID(undefined)} name='Obilasci' />
            {
            isLoggedIn && isAdmin &&
            <>
              <Menu.Item as={NavLink} to='/adminDashboard' name='Admin' />
              <Menu.Item as={NavLink} to='/errors' name='Errors' />
            </>
            }
            {
            isLoggedIn && isVodic &&
            <> 
              <Menu.Item>
                  <Button  positive content='Dodaj obilazak' as={NavLink} to='/createTour'/>
              </Menu.Item>
            </>
            }
            <Menu.Item position='right'>
              {
                isLoggedIn ? (
                  <>
                    <Image src={user?.slika || defaultUserImage} avatar spaced='right' />
                    <Dropdown pointing='top left' text={user?.userName}>
                      <Dropdown.Menu>
                        <Dropdown.Item as={Link} to={`/profiles/${user?.userName}`} text='Moj profil' icon='user' />
                        <Dropdown.Item onClick={loguot} text='Loguot' icon='power'/>
                      </Dropdown.Menu>
                      
                    </Dropdown>
                    </>
                 
                )
                : (
                  <Button positive content='Login' onClick={() => modalStore.openModal(<LoginForm/>)} />
                )
                
              }
              </Menu.Item>
            
        </Container>
    </Menu>
  )
})
