import {useAuth} from "../components/AuthProvider";
import {useEffect, useState} from "react";
import {ChecksService, CheckViewModel} from "../api";

function IndexPage() {
    const auth = useAuth()
    
    const [checks, setChecks] = useState<CheckViewModel[]>([])
    
    useEffect(() => {
        ChecksService.getApiChecks().then(d => setChecks(d))
    }, [])
    
    return <>
        Вы {auth.user?.id}
        <ol>
            {checks.map(c => <li>
                <ul>
                    {c.products?.map(p => <li>{p.name} {p.price}</li>)}
                </ul>
                <br/>
            </li>)}
        </ol>
    </>
}

export default IndexPage;