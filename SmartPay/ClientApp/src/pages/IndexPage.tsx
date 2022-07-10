import {useAuth} from "../components/AuthProvider";
import {useEffect, useState} from "react";
import {ChecksService, CheckViewModel} from "../api";
import '../styles/index.scss'
import {Link} from "react-router-dom";

function IndexPage() {
    return <>
        <div className="container index">
            <h1>Smart Pay</h1>
            <Link to={'/login'}>
                <button className="user">Пользователь</button>
            </Link>
            <Link to={'#'}>
                <button className="admin">Админка</button>
            </Link>
        </div>
    </>
}

export default IndexPage;