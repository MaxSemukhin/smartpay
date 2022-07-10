import '../styles/favourite_categories.scss'
import '../styles/favourite.css'
import {useEffect, useState} from "react";
import {CategoryViewModel, FavoriteService} from "../api";
import {useNavigate} from "react-router-dom";
import {upVariants} from "../animations";
import {AnimatePresence, motion} from 'framer-motion';

export interface Props {

}

function FavoriteCategoriesSelectPage(props: Props) {
    const navigate = useNavigate()
    
    const [categories, setCategories] = useState<CategoryViewModel[]>()
    const [selectedCategories, setSelectedCategories] = useState<CategoryViewModel[]>([])
    const [loading, setLoading] = useState(false)

    useEffect(() => {
        FavoriteService.getApiFavoriteCategoriesAll().then(d => setCategories(d))
    }, [])

    console.log(selectedCategories)

    const onSelect = (c: CategoryViewModel) => {
        if (!selectedCategories.includes(c)) {
            if (selectedCategories.length >= 3) return alert("Вы можете выбрать только 3 категории")
            setSelectedCategories([...selectedCategories, c])
        } else {
            setSelectedCategories(selectedCategories.filter(s => s != c))
        }
    }
    
    const onNext = () => {
        if (!loading) {
            FavoriteService.postApiFavoriteCategories(selectedCategories).then(() => {
                navigate('/favorite/merchants')
            })
        }
    }

    return <motion.div variants={upVariants} initial={'init'} animate={'show'} exit={'hide'} className={"layout"}>
        <div className="container categories">
                <motion.p layout>Выберите наиболее интересные для вас категории, мы начислим на них наивысший кэшбэк</motion.p>
    
                {categories?.map(c => <>
                    <motion.button initial={{opacity: 0, x: -20}} animate={{opacity: 1, x: 0}} layout className="food" onClick={() => onSelect(c)}>{c.name}</motion.button>
                    <AnimatePresence>
                        {selectedCategories.includes(c) && <motion.span initial={{scale: 0}} animate={{scale: 1}} exit={{scale: 0}} layout id='food_span' className='span'/>}
                    </AnimatePresence>
                    <br/>
                </>)}
    
                <motion.button layout className="next" onClick={onNext} disabled={loading || selectedCategories?.length == 0}>Далее</motion.button>
        </div>
    </motion.div>
}

export default FavoriteCategoriesSelectPage;
