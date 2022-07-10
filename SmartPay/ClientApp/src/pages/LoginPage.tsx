import {useAuth} from "../components/AuthProvider";
import {useEffect, useState} from "react";
import {AuthService} from "../api";
import {useNavigate} from "react-router-dom";
import {SearchOutline} from "react-ionicons";
import '../styles/uid.scss'

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
    
    const login = () => {
        console.log("Login")
        if (!loading) {
            const nubmberUserId = parseInt(userId);

            if (isNaN(nubmberUserId)) return;

            setLoading(true)

            AuthService.postApiAuthLoginId({userId: nubmberUserId}).then((d) => {
                auth.setToken(d.token)
            }).finally(() => setLoading(false))
        }
    }
    
    const onKey = (e: KeyboardEvent) => {
       if (e.key == 'Enter') login()
    }
    
    useEffect(() => {
        document.addEventListener("keydown", onKey)
        return () => document.removeEventListener("keydown", onKey)
    })
    
    return <>
        <div className="container login">
            <p>Для того, чтобы протестировать наш сервис предлагаем вам ввести один из предложенных id, который позволит
                просмотреть как будет выглядеть аккаунт пользователя</p>
            <input onChange={(e) => setUserId(e.target.value)} value={userId} disabled={loading} tabIndex={0} placeholder="Введите id" id="uid_textarea"></input>
            <SearchOutline onClick={login}/>
        </div>
    </>
}

export default LoginPage;