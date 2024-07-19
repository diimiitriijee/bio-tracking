import { useState, ReactElement, MouseEvent } from 'react';
import { Menu, Sidebar } from 'semantic-ui-react';

interface NavbarMbProps {
  renderLinks?: () => ReactElement[];
}

const Overlay = (): ReactElement => (
  <div style={{
    backgroundColor: "rgba(0, 0, 0, 0.795)",
    position: "fixed",
    height: "110vh",
    width: "100%",
  }} />
);

const HamIcon = (): ReactElement => (<i className="big bars icon inverted" />);
const CloseIcon = (): ReactElement => (<i className="big close red icon" />);

const NavbarMb = ({ renderLinks }: NavbarMbProps): ReactElement => {
  const [visible, setVisible] = useState(false);
  const [icon, setIcon] = useState<ReactElement>(HamIcon);
  const [activeItem, setActiveItem] = useState<string>("home");

  const handleItemClick = (e: MouseEvent<HTMLAnchorElement>, { name }: { name?: string }): void => {
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

  return (
    <>
      {visible && <Overlay />}
      <Menu inverted size="tiny" borderless attached>
        <Menu.Item>
          <img src="ghostblog.svg" width="35px" height="35px" alt="logo" />
        </Menu.Item>
        <Menu.Menu position='right'>
          <Menu.Item onClick={toggleSidebar}>
            {icon}
          </Menu.Item>
        </Menu.Menu>
      </Menu>
      <Sidebar as={Menu}
        animation='overlay'
        icon='labeled'
        inverted
        vertical
        visible={visible}
        width='thin'
      >
        <Menu.Item>
          <img src="ghostblog.svg" width="35px" height="35px" style={{ margin: "0 auto" }} alt="logo" />
        </Menu.Item>
        <Menu.Item
          name='home'
          active={activeItem === 'home'}
          onClick={handleItemClick}
        />
        <Menu.Item
          name='messages'
          active={activeItem === 'messages'}
          onClick={handleItemClick}
        />
        <Menu.Item
          name='friends'
          active={activeItem === 'friends'}
          onClick={handleItemClick}
        />
        <Menu.Item
          name='login'
          active={activeItem === 'login'}
          onClick={handleItemClick}
          position="right"
        />
        <Menu.Item
          name='sign_in'
          active={activeItem === 'sign_in'}
          onClick={handleItemClick}
        />
        {renderLinks && renderLinks().map((link, index) => (
          <Menu.Item key={index} onClick={hideSidebar}>
            {link}
          </Menu.Item>
        ))}
      </Sidebar>
    </>
  );
}

export default NavbarMb;
