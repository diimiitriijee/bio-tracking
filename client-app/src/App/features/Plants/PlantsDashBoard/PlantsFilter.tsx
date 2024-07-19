import { Header, Menu } from 'semantic-ui-react'
import { useStore } from '../../../stores/store'
import { observer } from 'mobx-react-lite'

export default observer( function PlantsFilter() {

    const {areaStore:{predicate, setPredicate}} = useStore()

  return (
    <>
        <Menu vertical size='large' style={{width:'100%', marginTop: 25}}>
            <Header icon='filter' attached color="teal" content='Filter' />
            <Menu.Item 
        content='Sve biljke'
        active={predicate.has('all')}
        onClick={() => setPredicate('all', true)}
    />
    <Menu.Item 
        content='Lekovite biljke' 
        active={predicate.has('Lekovita')}
        onClick={() => setPredicate('Lekovita', true)}
    />
    <Menu.Item 
        content='Nelekovite biljke' 
        active={predicate.has('Nelekovita')}
        onClick={() => setPredicate('Nelekovita', true)}
    />
        </Menu>
        <Header />
        
    </>
  )
})
