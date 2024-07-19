import { useEffect, useState } from 'react'
import useQuery from '../../util/hooks';
import { useStore } from '../../stores/store';
import agent from '../../api/agent';
import { toast } from 'react-toastify';
import { Button, Header, Icon, Segment } from 'semantic-ui-react';
import LoginForm from './LoginForm';

export default function ConfirmEmail() {
    const {modalStore} = useStore();
    const email = useQuery().get('email') as string;
    const token = useQuery().get('token') as string;

    const Status =  { 
        Verifying : 'Verifying',
        Failed : 'Failed',
        Success : 'Success'
    }

    const [status, setStatus] = useState(Status.Verifying);

    function handleConfirmEmailResend(){
        agent.Account.resendEmailConfirmed(email).then(() => {
            toast.success('Email za verifikaciju je ponovo poslat - molimo Vas proverite sanduce!');
        }).catch(error => console.log(error));
    }

    useEffect(() => { //pozivam kontroler odavde koji verifikuje mail
        agent.Account.verifyEmail(token, email).then(() => {
            setStatus(Status.Success);
        }).catch(() => {
            setStatus(Status.Failed);
        })
    }, [Status.Failed, Status.Success, token, email])

    function getBody() {
        switch(status) {
            case Status.Verifying:
                return <p>Verifikuje se...</p>
            case Status.Failed:
                return (
                    <div>
                        <p>Verifikacija nije uspela. Mozete da probate ponovno slanje linka za verifikaciju!</p>
                        <Button primary onClick = {handleConfirmEmailResend} size = 'huge' content= 'Ponovo posalji'/>
                    </div>
                )
            case Status.Success:
                return (
                    <div>
                    <p>Email je uspesno verifikovan! Sada mozete da se ulogujete u aplikaciju koristeci svoj nalog!</p>
                    <Button primary onClick = {() => modalStore.openModal(<LoginForm />)} size='huge' content ='Log in' />
                    </div>
                );
        }

    }
    
    return (
        <Segment placeholder textAlign='center'>
            <Header icon>
                <Icon name = 'envelope' />
                Verifikacija emaila
            </Header>
            <Segment.Inline>
                {getBody()}
            </Segment.Inline>
        </Segment>
    )

    
}