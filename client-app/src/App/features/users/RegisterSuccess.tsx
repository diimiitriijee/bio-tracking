import useQuery from '../../util/hooks';
import agent from '../../api/agent';
import { toast } from 'react-toastify';
import { Button, Header, Icon, Segment } from 'semantic-ui-react';

export default function RegisterSuccess() {
    const email = useQuery().get('email') as string;

    function handleConfirmEmailResend(){
        agent.Account.resendEmailConfirmed(email).then(() => {
            toast.success('Email za verifikaciju je ponovo poslat - molimo Vas proverite sanduce!');
        }).catch(error => console.log(error));
    }

    return (
        <Segment placeholder textAlign = 'center'>
            <Header icon color = 'green'>
                <Icon name = 'check'/>
                Uspesno ste se registrovali na sistem!
            </Header>
            <p>Molimo vas proverite svoje prijemno sanduce (i spam sekciju) za verifikacioni email</p>
            {email &&
                <>
                    <p>Niste primili verifikacioni email? Kliknite na dugme ispod.</p>
                    <Button primary onClick={handleConfirmEmailResend} content = 'Ponovo posalji' size = 'huge' />
                </>
            }
        </Segment>
    )
}