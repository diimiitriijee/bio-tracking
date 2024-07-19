import { Fragment } from "react/jsx-runtime";
import { Button, Placeholder, Segment } from "semantic-ui-react";

export default function PlantListItemPlaceholder() {
  return (
    <Fragment>
            <Placeholder fluid style={{ marginTop: 25 }}>
                <Segment.Group>
                    <Segment style={{ minHeight: 110 }}>
                        <Placeholder>
                            <Placeholder.Header image>
                                <Placeholder.Line />
                                <Placeholder.Line />
                            </Placeholder.Header>
                            <Placeholder.Paragraph>
                                <Placeholder.Line />
                            </Placeholder.Paragraph>
                        </Placeholder>
                    </Segment>
                    
                    <Segment clearing>
                        <Button disabled color='teal' floated='right' content='Detalji' />
                    </Segment>
                </Segment.Group>
            </Placeholder>
        </Fragment>
  )
}
