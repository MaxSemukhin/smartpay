import {ReactElement} from "react";
import {useAuth} from "./AuthProvider";

export interface Props {
    auntificated: ReactElement,
    nonAuntificated: ReactElement
}

function AuthSwitch(props: Props) {
    const auth = useAuth()
    
    return auth.init ? auth.isAuthenticated ? props.auntificated : props.nonAuntificated : <></>
}

export default AuthSwitch;