import '../styles/favourite_shops.scss'
import '../styles/favourite.css'

export interface Props {

}

function FavoriteShopsSelectPage(props: Props) {
    return <>
        <div className="container shops">
            <p className="text">Мы сотрудничаем с вашими любимыми магазинами, и мы можем предоставить вам <span>Максимальный кэшбек</span> в
                вашем любимом магазине. Какой магазин вы больше любите:</p>
            <button className="food">Продукты питания</button>
            <br/>

            <input type="checkbox" className="custom-checkbox" name="shop" value="yes"/>
            <label htmlFor="shop">Пятерочка</label>
            <br/>
            <input type="checkbox" className="custom-checkbox" name="shop" value="yes"/>
            <label htmlFor="shop">Магнит</label>
            <br/>

            <button className="electronics">Бытовая техника и электроника</button>
            <br/>

            <input type="checkbox" className="custom-checkbox" name="shop" value="yes"/>
            <label htmlFor="shop">Ситилинг</label>
            <br/>

            <button className="medicine">Лекарства и мед. центры</button>
            <br/>

            <input type="checkbox" className="custom-checkbox" name="shop" value="yes"/>
            <label htmlFor="shop">apteka.ru</label>
            <br/>

            <a href="favourite_shops.html">
                <button className="next">Далее</button>
            </a>
        </div>
    </>
}

export default FavoriteShopsSelectPage;