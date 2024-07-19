import { Fragment } from "react/jsx-runtime";
import { Header, List } from "semantic-ui-react";
import { useStore } from "../../../stores/store";
import GuideListItem from "./GuideListItem";

export default function GuideList() {
    const { adminStore } = useStore();
    const { guides } = adminStore;

    
    console.log(guides)
  return (
    <Fragment>
            <Header sub color='teal'>Lista zahteva vodica</Header>
            <List>
                {guides.map(guide => (
                    <GuideListItem key={guide.id} guide={guide} />
                ))}
            </List>
        </Fragment>
  )
}
