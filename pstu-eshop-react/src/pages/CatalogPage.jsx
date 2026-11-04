import { useEffect, useState } from "react";
import ProductCard from "../components/ProductCard";

export default function CatalogPage() {
    const [products, setProducts] = useState([]);
    const [categories, setCategories] = useState([]);
    const [category, setCategory] = useState("");
    const [sort, setSort] = useState("name_asc");

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    const [cartCount, setCartCount] = useState(0);
    const [adding, setAdding] = useState(false);
    const [cartError, setCartError] = useState("");

    useEffect(() => {
        const controller = new AbortController();

        async function loadInitialData() {
            try {
                const [categoriesResponse, cartResponse] =
                    await Promise.all([
                        fetch("/api/products/categories", {
                            signal: controller.signal
                        }),
                        fetch("/api/cart/summary", {
                            signal: controller.signal
                        })
                    ]);

                if (!categoriesResponse.ok || !cartResponse.ok)
                    throw new Error("Не удалось загрузить данные");

                const [categoryData, cartData] =
                    await Promise.all([
                        categoriesResponse.json(),
                        cartResponse.json()
                    ]);

                if (!controller.signal.aborted) {
                    setCategories(categoryData);
                    setCartCount(cartData.totalQuantity);
                }
            } catch (err) {
                if (!controller.signal.aborted)
                    setError(err.message);
            }
        }

        loadInitialData();

        return () => controller.abort();
    }, []);

    // Вызывается при изменении полей сортировки
    useEffect(() => {
        const controller = new AbortController();

        async function loadProducts() {
            setLoading(true);
            setError("");

            try {
                const params = new URLSearchParams();

                if (category)
                    params.set("category", category);

                params.set("sort", sort);

                const response = await fetch(
                    `/api/products?${params}`,
                    { signal: controller.signal }
                );

                if (!response.ok)
                    throw new Error(`HTTP ${response.status}`);

                const data = await response.json();

                if (!controller.signal.aborted)
                    setProducts(data);
            } catch (err) {
                if (!controller.signal.aborted)
                    setError(err.message);
            } finally {
                if (!controller.signal.aborted)
                    setLoading(false);
            }
        }

        loadProducts();

        return () => controller.abort();
    }, [category, sort]);

    async function addToCart(productId) {
        if (adding) return;

        setAdding(true);
        setCartError("");

        try {
            // Получаем antiforgery-токен
            const tokenResponse = await fetch(
                "/api/security/antiforgery"
            );

            if (!tokenResponse.ok)
                throw new Error("Ошибка получения токена");

            const { token } = await tokenResponse.json();

            // Добавляем товар в серверную сессию
            const response = await fetch("/api/cart/items", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "RequestVerificationToken": token
                },
                body: JSON.stringify({ productId })
            });

            if (!response.ok) {
                const details = await response.json().catch(() => null);

                throw new Error(details?.message ?? `HTTP ${response.status}`);
            }

            const data = await response.json();

            setCartCount(data.totalQuantity);
        } catch (err) {
            setCartError(err.message);
        } finally {
            setAdding(false);
        }
    }

    return (
        <main className="container py-4">
            <h1 className="mb-3">PSTU EShop React App</h1>

            <p className="mb-4">
                Корзина: <strong>{cartCount}</strong> товаров
            </p>

            {cartError && (
                <div className="alert alert-danger">
                    {cartError}
                </div>
            )}

            <div className="row g-3 mb-4">
                <div className="col-md-6">
                    <label className="form-label">
                        Категория
                    </label>

                    <select
                        className="form-select"
                        value={category}
                        onChange={event => setCategory(event.target.value)}
                    >
                        <option value="">Все категории</option>

                        {categories.map(item => (
                            <option key={item} value={item}>
                                {item}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="col-md-6">
                    <label className="form-label">
                        Сортировка
                    </label>

                    <select
                        className="form-select"
                        value={sort}
                        onChange={event => setSort(event.target.value)}
                    >
                        <option value="name_asc">
                            По названию (A-Z)
                        </option>
                        <option value="name_desc">
                            По названию (Z-A)
                        </option>
                        <option value="price_asc">
                            Сначала дешёвые
                        </option>
                        <option value="price_desc">
                            Сначала дорогие
                        </option>
                    </select>
                </div>
            </div>

            {loading && <p>Загрузка товаров...</p>}

            {error && (
                <div className="alert alert-danger">
                    {error}
                </div>
            )}

            {!loading && !error && (
                <>
                    <p>Найдено товаров: {products.length}</p>

                    <div className="row row-cols-1 row-cols-md-3 g-3">
                        {products.map(product => (
                            <ProductCard
                                key={product.id}
                                product={product}
                                onAdd={addToCart}
                                adding={adding}
                            />
                        ))}
                    </div>
                </>
            )}
        </main>
    );
}