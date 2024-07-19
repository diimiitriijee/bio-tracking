import { Fragment } from "react/jsx-runtime";
import { useStore } from "../../../stores/store";
import { Header } from "semantic-ui-react";
import RoutListItem from "./RoutListItem";

export default function RoutList() {
    const {routStore} = useStore();
    const {groupedRouts} = routStore; 
    console.log(groupedRouts)
  return (
    <>
    {
        groupedRouts.map(([group, routes])=>(
            <Fragment key={group}>
                <Header sub color='teal'>
                    {group}
                </Header>
                
                    {
                        routes.map(rout => (
                            <RoutListItem key={rout.id} rout={rout}/>
                        ))
                    }
     
            </Fragment>
        ))
    }
    </>
  )
}
