import {useAuth} from "../components/AuthProvider";
import {Navigate, useNavigate} from "react-router-dom";
import {useEffect} from "react";

function LogoutPage() {
    const auth = useAuth()
    const navigate = useNavigate()
    
    useEffect(() => {
        if (auth.init && auth.isAuthenticated) auth.setToken(null)
        if (auth.init && !auth.isAuthenticated) navigate('/login', {replace: true})
    }, [auth.isAuthenticated, auth.init])
    
    return <></>
}

export default LogoutPage;