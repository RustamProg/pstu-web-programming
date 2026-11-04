import { Link, Routes, Route } from "react-router";
import CatalogPage from "./pages/CatalogPage";
import CartPage from "./pages/CartPage";

export default function App() {
    return (
        <div className="container py-4">
            <nav className="d-flex gap-3 mb-4">
                <Link to="/">Каталог</Link>
                <Link to="/cart">Корзина</Link>
            </nav>

            <Routes>
                <Route path="/" element={<CatalogPage />} />
                <Route path="/cart" element={<CartPage />} />
            </Routes>
        </div>
    );
}