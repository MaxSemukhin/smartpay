import '../styles/favourite_categories.scss'
import '../styles/favourite.css'
import {useEffect, useState} from "react";
import {CategoryViewModel, FavoriteCategoriesService} from "../api";
import {useNavigate} from "react-router-dom";

export interface Props {

}

function FavoriteCategoriesSelectPage(props: Props) {
    const navigate = useNavigate()
    
    const [categories, setCategories] = useState<CategoryViewModel[]>()
    const [selectedCategories, setSelectedCategories] = useState<CategoryViewModel[]>([])
    const [loading, setLoading] = useState(false)

    useEffect(() => {
        FavoriteCategoriesService.getApiFavoritecategoriesAll().then(d => setCategories(d))
    }, [])

    console.log(selectedCategories)

    const onSelect = (c: CategoryViewModel) => {
        if (!selectedCategories.includes(c)) {
            if (selectedCategories.length >= 3) return alert("Вы можете выбрать только 3 категории")
            setSelectedCategories([...selectedCategories, c])
        }
    }
    
    const onNext = () => {
        if (!loading) {
            FavoriteCategoriesService.postApiFavoritecategories(selectedCategories).then(() => {
                navigate('/app')
            })
        }
    }

    return <>
        <div className="container categories">
            <p>Выберите наиболее интересные для вас категории, мы начислим на них наивысший кэшбэк</p>

            {categories?.map(c => <>
                <button className="food" onClick={() => onSelect(c)}>{c.name}</button>
                {selectedCategories.includes(c) && <span id='food_span' className='span'/>}
                <br/>
            </>)}

            <button className="next" onClick={onNext} disabled={loading || selectedCategories?.length == 0}>Далее</button>
        </div>
    </>
}

export default FavoriteCategoriesSelectPage;
