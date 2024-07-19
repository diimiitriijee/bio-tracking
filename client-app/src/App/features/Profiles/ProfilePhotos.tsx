import { SyntheticEvent, useState } from 'react'
import { Card, Header, Tab, Image, Grid, Button } from 'semantic-ui-react'
import { Photo, Profil } from '../../modules/Profil';
import { useStore } from '../../stores/store';
import PhotoUploadWidget from '../../common/imageUpload/PhotoUploadWidget';
import { observer } from 'mobx-react-lite';

interface Props{
    profile:Profil;
  }

export default observer( function ProfilePhotos({profile}:Props) {

    const {profileStore:{isCurrentUser, uploadPhoto, uploading, loading, setMainPhoto, deletePhoto}} = useStore();
    const [addPhotoMode,setAddPhotoMode] = useState(false);
    const [target, setTarget] = useState('');

    function handlePhotoUpload(file:Blob){
        uploadPhoto(file).then(()=> setAddPhotoMode(false))
    }

    function handleSetMainPhoto(photo: Photo, e: SyntheticEvent<HTMLButtonElement>){
        setTarget(e.currentTarget.name);
        setMainPhoto(photo);
    }

    function handleDeletePhoto(photo: Photo, e: SyntheticEvent<HTMLButtonElement>){
        setTarget(e.currentTarget.name);
        deletePhoto(photo);
    }

  return (
    <Tab.Pane>
        <Grid>
            <Grid.Column width={16}>
                <Header floated='left' icon='image' content='Photos' />
                {isCurrentUser &&(
                    <Button floated='right' basic content={addPhotoMode? 'Cancel':'Add Photo'} onClick={()=>setAddPhotoMode(!addPhotoMode)} />
                )
                }
            </Grid.Column>
            <Grid.Column  width={16}>
                {addPhotoMode?(
                    <PhotoUploadWidget uploadPhoto={handlePhotoUpload} loading={uploading} />
                ):(
                    <Card.Group>
                        {profile.slike?.map(photo =>(
                            <Card key={photo.id}>
                                <Image src={photo.url}/>
                                { isCurrentUser && (
                                    <Button.Group fuild widths={2}>
                                        <Button 
                                            basic
                                            color='green'
                                            content = 'Profilna'
                                            name={'main' + photo.id}
                                            disabled = {photo.isMain}
                                            loading={target === 'main' + photo.id && loading}
                                            onClick={e => handleSetMainPhoto(photo,e)}
                                        />
                                        <Button 
                                            basic 
                                            color='red' 
                                            icon='trash' 
                                            loading={target === photo.id && loading}
                                            onClick={e=>handleDeletePhoto(photo,e)}
                                            disabled={photo.isMain}
                                            name={photo.id}
                                            />
                                    </Button.Group>
                                )

                                }
                            </Card>
                        ))}
                        
                    </Card.Group>
                )
                }
            </Grid.Column>
        </Grid>
        
    </Tab.Pane>
  )
})
