import {useAuth} from "../components/AuthProvider";
import {useEffect, useState} from "react";
import {AuthService} from "../api";
import {useNavigate} from "react-router-dom";

export interface Props {

}

function LoginPage(props: Props) {
    const auth = useAuth()
    const [userId, setUserId] = useState<string>('')
    const [loading, setLoading] = useState(false)
    const navigate = useNavigate()
    
    useEffect(() => {
        if (auth.init && auth.isAuthenticated) navigate('/', {replace: true})
    }, [auth.isAuthenticated])
    
    return <>
        <input placeholder={"Id"} onChange={(e) => setUserId(e.target.value)} value={userId}/>
        <button disabled={loading} onClick={() => {
            const nubmberUserId = parseInt(userId);
            
            if (nubmberUserId == NaN) return;
            
            setLoading(true)
            
            AuthService.postApiAuthLoginId({userId: nubmberUserId}).then((d) => {
                auth.setToken(d.token)
            }).finally(() => setLoading(false))
        }}>Войти</button>
    </>
}

export default LoginPage;