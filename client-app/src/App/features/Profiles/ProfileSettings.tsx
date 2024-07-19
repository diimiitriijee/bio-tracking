import { useState } from "react";
import { Grid, Header, Form, Button, Tab, Message, Modal } from "semantic-ui-react";
import { observer } from "mobx-react-lite";
import { useStore } from "../../stores/store";

export default observer(function ProfileSettings() {
    const [chngPassword, setChangePassword] = useState(false);
    const [oldPassword, setOldPassword] = useState('');
    const [newPassword, setNewPassword] = useState('');
    const [confirmedNewPassword, setConfirmPassword] = useState('');
    const [error, setError] = useState<string | null>(null);
    const [success, setSuccess] = useState<string | null>(null);
    const {userStore : {changePassword}} = useStore();
    const [open, setOpen] = useState(false);

    const validatePassword = (password: string) => {
        const hasUpperCase = /[A-Z]/.test(password);
        const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(password);
        const hasNumber = /[0-9]/.test(password);

        return hasUpperCase && hasSpecialChar && hasNumber;
    }

    const handleSubmit = async () => {
        if (newPassword !== confirmedNewPassword) {
            setError('Nova lozinka i potvrda lozinke se ne poklapaju');
            alert('Nova lozinka i potvrda lozinke se ne poklapaju');
            return;
        }

        if (newPassword === oldPassword) {
            setError('Nova lozinka ne sme biti ista kao stara lozinka');
            alert('Nova lozinka ne sme biti ista kao stara lozinka');
            return;
        }

        if (!validatePassword(newPassword)) {
            setError('Nova lozinka mora sadržati bar jedno veliko slovo, specijalni karakter i broj');
            alert('Nova lozinka mora sadržati bar jedno veliko slovo, specijalni karakter i broj');
            return;
        }

        setError(null);
        setSuccess(null);

        try {
            console.log("Odavde old", oldPassword);
            await changePassword(oldPassword, newPassword, confirmedNewPassword);
            setSuccess('Lozinka je uspešno promenjena');
                setChangePassword(!chngPassword)
                setOpen(true);
            
                
        } catch (error) {
            setError('Greska pri promeni lozinke: ' );
        }
    }
    const handleClose = () => {
        setOpen(false);
      };

    const renderChangePasswordForm = () => (
        <Form onSubmit={handleSubmit}>
            <Form.Input
                label='Stara lozinka'
                type='password'
                placeholder='Unesite staru lozinku'
                value={oldPassword}
                onChange={(e) => setOldPassword(e.target.value)}
                required
            />
            <Form.Input
                label='Nova lozinka'
                type='password'
                placeholder='Unesite novu lozinku'
                value={newPassword}
                onChange={(e) => setNewPassword(e.target.value)}
                required
            />
            <Form.Input
                label='Potvrdi novu lozinku'
                type='password'
                placeholder='Ponovo unesite novu lozinku'
                value={confirmedNewPassword}
                onChange={(e) => setConfirmPassword(e.target.value)}
                required
            />
            {error && <Message error content={error} />}
            {success && <Message success content={success} />}
            <Button type='submit' primary>Promeni lozinku</Button>
        </Form>
    );

    return (
        <Tab.Pane>
            <Grid>
                <Grid.Column width={16}>
                    <Header floated="left" icon='settings' content={'Podešavanja naloga'} />
                </Grid.Column>
                <Grid.Column width={16}>
                    <Button
                        primary
                        content={chngPassword ? 'Otkaži' : 'Promeni lozinku'}
                        onClick={() => setChangePassword(!chngPassword)}
                    />
                    {chngPassword && renderChangePasswordForm()}
                    <Modal
                    open={open}
                    onClose={handleClose}
                    size='small'
                >
                    <Header icon='check' content='Promena Lozinke' />
                    <Modal.Content>
                    <p>Uspešno promenjena lozinka</p>
                    </Modal.Content>
                    <Modal.Actions>
                    <Button color='green' onClick={handleClose}>
                        OK
                    </Button>
                    </Modal.Actions>
                </Modal>
                </Grid.Column>
            </Grid>
        </Tab.Pane>
    )
})
