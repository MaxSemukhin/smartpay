import logo from './logo.svg'
import {useState} from 'react'
import {Link, Navigate, Outlet, Route, Routes, useLocation} from "react-router-dom";

import {AnimatePresence} from "framer-motion";

function App() {
    const location = useLocation();

    return (
        <AnimatePresence exitBeforeEnter>
            <Routes>
                <Route path="/" element={<p>Test</p>}/>
            </Routes>
        </AnimatePresence>
    )
}

export default App
