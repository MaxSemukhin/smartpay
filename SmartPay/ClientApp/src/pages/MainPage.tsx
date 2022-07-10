import {useAuth} from "../components/AuthProvider";
import {useEffect, useState} from "react";
import {
    Category,
    CategoryViewModel,
    ChecksService,
    CheckViewModel, FavoriteService,
    Recommendation,
    RecommendationsService
} from "../api";
import '../styles/main.css'
import {Link, useNavigate} from "react-router-dom";
import { motion } from "framer-motion";
import {enterUp, exitUp, initialUp, translition, upVariants} from "../animations";

export interface Props {

}

function MainPage(props: Props) {
    const auth = useAuth()
    const navigate = useNavigate()

    const [checks, setChecks] = useState<CheckViewModel[]>([])
    const [favoriteCategories, setFavoriteCategories] = useState<CategoryViewModel[]>([])
    const [recommendations, setRecommendations] = useState<Recommendation[]>([])
    
    const [loadedImages, setLoadedImages] = useState<number[]>([])

    useEffect(() => {
        ChecksService.getApiChecks().then(d => setChecks(d))
        FavoriteService.getApiFavoriteCategories().then(d => {
            setFavoriteCategories(d)
            if (d.length == 0) navigate('/favorite/categories')
        })
        RecommendationsService.getApiRecommendations().then(d => setRecommendations(d))
    }, [])

    return <motion.div variants={upVariants} initial={'init'} animate={'show'} exit={'hide'} style={{width: '100%', minHeight: '100vh'}}>
        <div className="navbar">
            <div className="back">
                <Link to={'/logout'} className="back_btn">←</Link>
            </div>
            <div className="menu_elements">
                <Link to={'/favorite/categories'} className="menu_element category">Категории</Link>
                <a href="#" className="menu_element shop disabled">Магазины</a>
                <a href="#" className="menu_element helper disabled">Фин. Помощник</a>
            </div>
        </div>

        <div className="container main">
            <div className="row">
                {recommendations.map((r, i) => <div className="col-lg-4 col-sm-12">
                    <div className="card">
                        <motion.img layout onLoad={() => setLoadedImages(loadedImages.concat(i))} src={r.product?.category?.imageUrl ? 'img/' + r.product?.category?.imageUrl : null || "img/place.jpeg"} className="card-img-top" alt="..."/>
                        <motion.div layout className="card-body">
                            <h5 className="card-title">{r.product?.name}</h5>
                            <p className="card-text">Рекоменуем вам этот товар</p>
                            <p className="card-text">{r.product?.price} ₽</p>
                            <a onClick={() => alert("Я карта 🗺")} href="#" className="btn btn-primary">Купить</a>
                        </motion.div>
                    </div>
                </div>)}
            </div>
        </div>
    </motion.div>
}

export default MainPage;
