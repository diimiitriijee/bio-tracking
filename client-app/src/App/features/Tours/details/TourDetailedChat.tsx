import { Formik, Form, Field, FieldProps } from 'formik';
import { observer } from 'mobx-react-lite'
import { useEffect } from 'react'
import { Link } from 'react-router-dom';
import { Segment, Header, Comment, Loader } from 'semantic-ui-react'
import { useStore } from '../../../stores/store';
import * as Yup from 'yup';
import { formatDistanceToNow } from 'date-fns';

interface Props {
    obilazakId: string;
}

export default observer(function TourDetailedChat({ obilazakId }: Props) {
    const { commentStore } = useStore();
    const { userStore: { isLoggedIn } } = useStore();


    useEffect(() => {
        if (obilazakId && isLoggedIn) {
            commentStore.createHubConnection(obilazakId);
        }
        return () => {
            commentStore.clearComments();
        }
    }, [commentStore, obilazakId, isLoggedIn]);

    return ( isLoggedIn?
        <>
            <Segment
                textAlign='center'
                attached='top'
                inverted
                color='teal'
                style={{ border: 'none' }}
            >
                <Header>Komentarisi obilazak</Header>
            </Segment>
            <Segment attached clearing>
                <Formik
                    onSubmit={(values, { resetForm }) =>
                        commentStore.addComment(values).then(() => resetForm())}
                    initialValues={{ tekst: '' }}
                    validationSchema={Yup.object({
                        tekst: Yup.string().required()
                    })}
                >
                    {({ isSubmitting, isValid, handleSubmit }) => (
                        <Form className='ui form'>
                            <Field name='tekst'>
                                {(props: FieldProps) => (
                                    <div style={{ position: 'relative' }}>
                                        <Loader active={isSubmitting} />
                                        <textarea
                                            placeholder='Unesi komentar (Enter da se posalje, SHIFT + Enter za novi red)'
                                            rows={2}
                                            {...props.field}
                                            onKeyPress={e => {
                                                if (e.key === 'Enter' && e.shiftKey) {
                                                    return;
                                                }
                                                if (e.key === 'Enter' && !e.shiftKey) {
                                                    e.preventDefault();
                                                    isValid && handleSubmit();
                                                }
                                            }}
                                        />
                                    </div>
                                )}
                            </Field>
                        </Form>
                    )}
                </Formik>
                <Comment.Group>
                    {commentStore.comments.map(comment => (
                        <Comment key={comment.id}>
                            <Comment.Avatar src={comment.slikaProfila || '/src/assets/Images/user.png'} />
                            <Comment.Content>
                                <Comment.Author as={Link} to={`/profiles/${comment.username}`}>{comment.ime}</Comment.Author>
                                <Comment.Metadata>
                                    <div>{formatDistanceToNow(comment.datumKreiranja)} ago</div>
                                </Comment.Metadata>
                                <Comment.Text style={{ whiteSpace: 'pre-wrap' }}>{comment.tekst}</Comment.Text>
                            </Comment.Content>
                        </Comment>
                    ))}


                </Comment.Group>
            </Segment>
        </>
        :
        <>
            <Segment
                textAlign='center'
                attached='top'
                inverted
                color='teal'
                style={{ border: 'none' }}
            >
                <Header>Da biste se prijavili za obilazak morate da budete ulogovani!</Header>
            </Segment>
        </>
    )
})