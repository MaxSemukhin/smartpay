import logo from './logo.svg'
import {useState} from 'react'
import {Link, Navigate, Outlet, Route, Routes, useLocation} from "react-router-dom";

import {AnimatePresence} from "framer-motion";
import IndexPage from "./pages/IndexPage";
import AuthProvider from "./components/AuthProvider";
import LoginPage from "./pages/LoginPage";
import AuthSwitch from "./components/AuthSwitch";

function App() {
    const location = useLocation();

    return (
        <AuthProvider>
            <AnimatePresence exitBeforeEnter>
                <Routes>
                    <Route path="/" element={
                        <AuthSwitch
                            auntificated={<IndexPage/>}
                            nonAuntificated={<Navigate to={'/login'} replace={true}
                        />
                    }/>}/>
                    <Route path="login" element={<LoginPage/>}/>
                </Routes>
            </AnimatePresence>
        </AuthProvider>
    )
}

export default App
