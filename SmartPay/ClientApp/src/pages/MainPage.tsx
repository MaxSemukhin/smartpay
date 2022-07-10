import {useAuth} from "../components/AuthProvider";
import {useEffect, useState} from "react";
import {
    Category,
    CategoryViewModel,
    ChecksService,
    CheckViewModel,
    FavoriteCategoriesService, Recommendation,
    RecommendationsService
} from "../api";
import '../styles/main.css'
import {Link, useNavigate} from "react-router-dom";

export interface Props {

}

function MainPage(props: Props) {
    const auth = useAuth()
    const navigate = useNavigate()

    const [checks, setChecks] = useState<CheckViewModel[]>([])
    const [favoriteCategories, setFavoriteCategories] = useState<CategoryViewModel[]>([])
    const [recommendations, setRecommendations] = useState<Recommendation[]>([])

    useEffect(() => {
        ChecksService.getApiChecks().then(d => setChecks(d))
        FavoriteCategoriesService.getApiFavoritecategories().then(d => {
            setFavoriteCategories(d)
            if (d.length == 0) navigate('/favorite/categories')
        })
        RecommendationsService.getApiRecommendations().then(d => setRecommendations(d))
    }, [])

    return <>
        <div className="navbar">
            <div className="back">
                <Link to={'/logout'} className="back_btn">←</Link>
            </div>
            <div className="menu_elements">
                <a href="#" className="menu_element category">Категории</a>
                <a href="#" className="menu_element shop">Магазины</a>
                <a href="#" className="menu_element helper">Фин. Помощник</a>
            </div>
        </div>

        <div className="container main">
            <div className="row">
                {recommendations.map((r) => <div className="col-lg-4 col-sm-12">
                    <p>{r.product?.name}</p>
                </div>)}
            </div>
        </div>
    </>
}

export default MainPage;