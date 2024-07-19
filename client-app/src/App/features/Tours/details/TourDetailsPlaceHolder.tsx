import { Fragment } from "react/jsx-runtime";
import { Button, Grid, Placeholder, Segment } from "semantic-ui-react";

export default function TourDetailsPlaceHolder() {
  return (
    <Grid style={{ marginTop: '7em' }}>
    <Grid.Column width={10}>
      <Segment.Group>
        <Segment basic attached='top' style={{ padding: '0', height: '400px', backgroundColor: '#f3f3f3' }}>
          
        </Segment>
        <Segment basic>
          <Placeholder>
            <Placeholder.Header>
              <Placeholder.Line length='medium' />
              <Placeholder.Line length='full' />
            </Placeholder.Header>
            <Placeholder.Paragraph>
              <Placeholder.Line length='full' />
              <Placeholder.Line length='full' />
              <Placeholder.Line length='full' />
              <Placeholder.Line length='medium' />
            </Placeholder.Paragraph>
          </Placeholder>
        </Segment>
        <Segment clearing attached='bottom'>
          <Placeholder>
            <Placeholder.Paragraph>
              <Placeholder.Line length='medium' />
              <Placeholder.Line length='short' />
            </Placeholder.Paragraph>
          </Placeholder>
          <Button disabled color='teal' floated='right'>
            Pregled rute
          </Button>
        </Segment>
        <Segment basic>
          <Placeholder>
            
            <Placeholder.Paragraph>
              <Placeholder.Line length='full' />
              <Placeholder.Line length='full' />
              <Placeholder.Line length='medium' />
            </Placeholder.Paragraph>
          </Placeholder>
        </Segment>
      </Segment.Group>
    </Grid.Column>
    <Grid.Column width={6}>
      <Segment>
        <Placeholder>
          <Placeholder.Header>
            <Placeholder.Line length='short' />
            <Placeholder.Line length='very short' />
          </Placeholder.Header>
          <Placeholder.Paragraph>
            <Placeholder.Line length='medium' />
            <Placeholder.Line length='short' />
            <Placeholder.Line length='short' />
          </Placeholder.Paragraph>
        </Placeholder>
      </Segment>
    </Grid.Column>
  </Grid>
  )
}
