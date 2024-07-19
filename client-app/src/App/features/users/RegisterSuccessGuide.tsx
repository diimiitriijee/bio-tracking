//import useQuery from '../../util/hooks';
import { Header, Icon, Segment } from 'semantic-ui-react';

export default function RegisterSuccess() {
    //const email = useQuery().get('email') as string;

    return (
        <Segment placeholder textAlign = 'center'>
            <Header icon color = 'green'>
                <Icon name = 'check'/>
                Uspesno ste podneli zahtev za kreiranje naloga vodica!
            </Header>
            <p>Vas zahtev je prosledjen administratorima na proveru i obradu. Molimo Vas za strpljenje</p>
            <p>Administratori aplikacije ce Vam odgovoriti u najkracem roku, dobicete obavestenje putem emaila kada Vas zahtev bude obradjen</p>
            <p>U slucaju prihvatanja, Vas nalog bice kreiran uz lozinku koja ce Vam biti poslata u prilogu emaila zajedno sa linkom za verifikaciju naloga</p>
            <p><b>Potrebno je promeniti lozinku nakon sto prvi put pristupite sistemu!</b></p>
            <p>Molimo vas proveravajte kako svoje prijemno sanduce tako i spam sekciju za verifikacioni email</p>
        </Segment>
    )
}