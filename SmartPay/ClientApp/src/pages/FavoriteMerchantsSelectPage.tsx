import '../styles/favourite_shops.scss'
import '../styles/favourite.css'
import {useAuth} from "../components/AuthProvider";
import {useNavigate} from "react-router-dom";
import {ChangeEvent, useEffect, useState} from "react";
import {
    CategoryViewModel,
    ChecksService,
    CheckViewModel,
    FavoriteService, Merchant, MerchantCategory, MerchantViewModel,
    Recommendation,
    RecommendationsService
} from "../api";
import {upVariants} from "../animations";
import {motion} from 'framer-motion';
import {Checkbox} from "react-ionicons";

export interface Props {

}

function FavoriteMerchantsSelectPage(props: Props) {
    const navigate = useNavigate()
    const [favoriteCategories, setFavoriteCategories] = useState<CategoryViewModel[]>([])
    const [selectedMerchants, setSelectedMerchants] = useState<MerchantViewModel[]>([])
    const [list, setList] = useState<MerchantCategory[]>([])

    useEffect(() => {
        FavoriteService.getApiFavoriteCategories().then(d => {
            setFavoriteCategories(d)
            if (d.length == 0) navigate('/favorite/categories')
        })
        FavoriteService.getApiFavoriteMerchantsAll().then(d => setList(d))
    }, [])
    
    const post = () => {
        FavoriteService.postApiFavoriteMerchants(selectedMerchants).finally(() => navigate('/app'))
    }
    
    const check = (e: any, m: MerchantViewModel) => {
        if (e.target.checked && !selectedMerchants.includes(m)) {
            setSelectedMerchants(selectedMerchants.concat(m))
        } else {
            setSelectedMerchants(selectedMerchants.filter(s => s != m))
        }
    }
    

    return <motion.div variants={upVariants} initial={'init'} animate={'show'} exit={'hide'} className={"layout"}>
        <motion.div className="container shops">
            <motion.p layout className="text">Мы сотрудничаем с вашими любимыми магазинами, и мы можем предоставить
                вам <span>Максимальный кэшбек</span> в
                вашем любимом магазине. Какой магазин вы больше любите:
            </motion.p>

            {list.map(l => <motion.div>
                <button className="food">{l.name}</button>
                <br/>
                {l.merchants?.map(m => <motion.div>
                    <input checked={selectedMerchants.includes(m)} onChange={(e) => check(e, m)} type="checkbox" className="custom-checkbox" name="shop" value="yes"/>
                    <label htmlFor="shop">{m.name}</label>
                    <br/>
                </motion.div>)}
            </motion.div>)}
            <motion.button layout className="next" onClick={post}>Готово</motion.button>
        </motion.div>
    </motion.div>
}

export default FavoriteMerchantsSelectPage;