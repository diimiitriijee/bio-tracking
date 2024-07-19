import { useState, ReactElement, MouseEvent } from 'react';
import { Button, Container, Dropdown, Image, Menu, Sidebar } from 'semantic-ui-react';
import { Link, NavLink, useNavigate } from 'react-router-dom';
import { useStore } from '../../stores/store';
import { observer } from 'mobx-react-lite';
import LoginForm from '../users/LoginForm';
import defaultUserImage from '../../../assets/Images/user.png';
import { useMediaQuery } from 'react-responsive';
import './NavBar.css'

const Overlay = (): ReactElement => (
  <div style={{
    backgroundColor: "rgba(0, 0, 0, 0.795)",
    position: "fixed",
    height: "110vh",
    width: "100%",
    zIndex: 10,
  }} />
);

const HamIcon = (): ReactElement => (<i className="big bars icon inverted" />);
const CloseIcon = (): ReactElement => (<i className="big close red icon" />);

const Navbar = observer((): ReactElement => {
  const isMobile = useMediaQuery({ maxWidth: 767 });
  const { userStore: { isLoggedIn, user, loguot, isAdmin, isVodic }, modalStore } = useStore();
  const { tourStore: { setAreaID } } = useStore();
  const [visible, setVisible] = useState(false);
  const [icon, setIcon] = useState<ReactElement>(HamIcon);
  const [activeItem, setActiveItem] = useState<string>("home");
  const navigate = useNavigate();

  const handleItemClick = (_e: MouseEvent<HTMLAnchorElement>, { name }: { name?: string }): void => {
    if (name) {
      setActiveItem(name);
      hideSidebar();
    }
  };

  const hideSidebar = (): void => {
    setIcon(HamIcon);
    setVisible(false);
  };

  const showSidebar = (): void => {
    setIcon(CloseIcon);
    setVisible(true);
  };

  const toggleSidebar = (): void => {
    visible ? hideSidebar() : showSidebar();
  };

  const handleLoguot = () =>{
    loguot();
    navigate('/');
  }

  return (
    <>
      {isMobile ? (
        <>
          {visible && <Overlay />}
          <Menu inverted size="tiny" borderless attached style={{color:'green'}}>
            <Container>
              <Menu.Menu position='right'>
                <Menu.Item onClick={toggleSidebar}>
                  {icon}
                </Menu.Item>
              </Menu.Menu>
            </Container>
          </Menu>
          <Sidebar as={Menu}
            animation='overlay'
            icon='labeled'
            inverted
            vertical
            visible={visible}
            width='thin'
          >
            
            <Menu.Item
              as={NavLink} to=''
              name='Pocetna stranica'
              active={activeItem === 'home'}
              onClick={handleItemClick}
            />
            <Menu.Item
              as={NavLink} to='/allTours'
              name='Obilasci'
              active={activeItem === 'Obilasci'}
              onClick={(e, data) => {
                setAreaID(undefined);
                handleItemClick(e, data);
              }}
            />
            {isLoggedIn && isAdmin && (
              <>
                <Menu.Item
                  as={NavLink} to='/adminDashboard'
                  name='Admin'
                  active={activeItem === 'Admin'}
                  onClick={handleItemClick}
                />
                <Menu.Item
                  as={NavLink} to='/errors'
                  name='Errors'
                  active={activeItem === 'Errors'}
                  onClick={handleItemClick}
                />
              </>
            )}
            
            <Menu.Item position='right'>
              {isLoggedIn ? (
                <>
                  <Image src={user?.slika || defaultUserImage} avatar spaced='right' />
                  <Dropdown pointing='top left' text={user?.userName}>
                    <Dropdown.Menu>
                      <Dropdown.Item as={Link} to={`/profiles/${user?.userName}`} text='Moj profil' icon='user' />
                      <Dropdown.Item onClick={loguot} text='Odjavi se' icon='power'  />
                    </Dropdown.Menu>
                  </Dropdown>
                </>
              ) : (
                <Button positive content='Prijavi se' onClick={() => modalStore.openModal(<LoginForm />)} />
              )}
            </Menu.Item>
          </Sidebar>
        </>
      ) : (
        <Menu inverted fixed="top">
          <Container>
            <Menu.Item as={NavLink} to='' name='Pocetna stranica' />
            <Menu.Item as={NavLink} to='/allTours' onClick={() => setAreaID(undefined)} name='Obilasci' />
            {isLoggedIn && isAdmin && (
              <>
                <Menu.Item as={NavLink} to='/adminDashboard' name='Svi korisnici' />
                <Menu.Item as={NavLink} to='/guideRequests' name='Zahtevi vodica' />
              </>
              
            )}
            
            <Menu.Item position='right'>
              {isLoggedIn ? (
                <>
                  <Image src={user?.slika || defaultUserImage} avatar spaced='right' />
                  <Dropdown pointing='top left' text={user?.userName}>
                    <Dropdown.Menu>
                      <Dropdown.Item as={Link} to={`/profiles/${user?.userName}`} text='Moj profil' icon='user' />
                      <Dropdown.Item onClick={loguot} text='Odjavi se' icon='power' />
                    </Dropdown.Menu>
                  </Dropdown>
                </>
              ) : (
                <Button positive content='Prijavi se' onClick={() => modalStore.openModal(<LoginForm />)} />
              )}
            </Menu.Item>
          </Container>
        </Menu>
      )}
    </>
  );
});

export default Navbar;
