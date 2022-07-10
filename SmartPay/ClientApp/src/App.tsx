import logo from './logo.svg'
import {useState} from 'react'
import {Link, Navigate, Outlet, Route, Routes, useLocation} from "react-router-dom";

import {AnimatePresence} from "framer-motion";
import IndexPage from "./pages/IndexPage";
import AuthProvider from "./components/AuthProvider";
import LoginPage from "./pages/LoginPage";
import AuthSwitch from "./components/AuthSwitch";
import LogoutPage from "./pages/LogoutPage";
import 'bootstrap/dist/css/bootstrap.min.css';
import FavoriteMerchantsSelectPage from "./pages/FavoriteMerchantsSelectPage";
import FavoriteCategoriesSelectPage from "./pages/FavoriteCategoriesSelectPage";
import './styles/menu.css'
import MainPage from "./pages/MainPage";

function App() {
    const location = useLocation();

    return (
        <AuthProvider>
            <AnimatePresence exitBeforeEnter>
                <Routes key={location.pathname} location={location}>
                    <Route path="/" element={
                        <AuthSwitch
                            auntificated={<Navigate to={'/app'} replace={true}/>}
                            nonAuntificated={<IndexPage/>}/>}
                    />
                    <Route path="login" element={<LoginPage/>}/>
                    <Route path="logout" element={<LogoutPage/>}/>
                    <Route path="favorite/categories" element={<AuthSwitch
                        auntificated={<FavoriteCategoriesSelectPage/>}
                        nonAuntificated={<Navigate to={'/login'} replace={true}/>}/>
                    }/>
                    <Route path="favorite/merchants" element={ <AuthSwitch
                        auntificated={<FavoriteMerchantsSelectPage/>}
                        nonAuntificated={<Navigate to={'/login'} replace={true}/>}/>}/>
                    <Route path="app" element={
                        <AuthSwitch
                            auntificated={<MainPage/>}
                            nonAuntificated={<Navigate to={'/login'} replace={true}/>}/>}
                    />
                </Routes>
            </AnimatePresence>
        </AuthProvider>
    )
}

export default App
