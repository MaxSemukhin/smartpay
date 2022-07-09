import {useAuth} from "../components/AuthProvider";
import {useState} from "react";
import {AuthService} from "../api";

export interface Props {

}

function LoginPage(props: Props) {
    const auth = useAuth()
    const [userId, setUserId] = useState<string>('')
    
    return <>
        <input placeholder={"Id"} onChange={(e) => setUserId(e.target.value)} value={userId}/>
        <button onClick={() => {
            const nubmberUserId = parseInt(userId);
            
            if (nubmberUserId == NaN) return;
            
            AuthService.postApiAuthLoginId({userId: nubmberUserId}).then((d) => {
                auth.setToken(d.token)
            })
        }}>Войти</button>
    </>
}

export default LoginPage;