import { useEffect, useState } from "react";
import { Link } from "react-router";
import { apiJson } from "../api/eshopApi";

const emptyCart = {
    items: [],
    totalPrice: 0
};

export default function CartPage() {
    const [cart, setCart] = useState(emptyCart);

    const [cities, setCities] = useState([]);
    const [cityCode, setCityCode] = useState("");
    const [savedCity, setSavedCity] = useState("");

    const [loading, setLoading] = useState(true);
    const [busy, setBusy] = useState(false);
    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");

    useEffect(() => {
        const controller = new AbortController();

        async function loadData() {
            try {
                const [cartData, cityData] = await Promise.all([
                    apiJson("/api/cart", {
                        signal: controller.signal
                    }),
                    apiJson("/api/city", {
                        signal: controller.signal
                    })
                ]);

                if (controller.signal.aborted) return;

                setCart(cartData);
                setCities(cityData.cities);
                setCityCode(cityData.selectedCity ?? "");
                setSavedCity(cityData.selectedCity ?? "");
            } catch (err) {
                if (!controller.signal.aborted)
                    setError(err.message);
            } finally {
                if (!controller.signal.aborted)
                    setLoading(false);
            }
        }

        loadData();

        return () => controller.abort();
    }, []);

    async function changeCart(url, options) {
        if (busy) return;

        setBusy(true);
        setError("");

        try {
            const updatedCart = await apiJson(url, options);
            setCart(updatedCart);
        } catch (err) {
            setError(err.message);
        } finally {
            setBusy(false);
        }
    }

    function updateQuantity(productId, quantity) {
        if (quantity < 1 || quantity > 10) return;

        changeCart(`/api/cart/items/${productId}`, {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({ quantity })
        });
    }

    function removeItem(productId) {
        changeCart(`/api/cart/items/${productId}`, {
            method: "DELETE"
        });
    }

    async function saveCity() {
        if (!cityCode || busy) return;

        setBusy(true);
        setError("");

        try {
            const result = await apiJson("/api/city", { // Вызывается эндпоинт, который просто куку вставляет в заголовки
                method: "POST",
                headers: {
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ city: cityCode })
            });

            setSavedCity(result.selectedCity);
        } catch (err) {
            setError(err.message);
        } finally {
            setBusy(false);
        }
    }

    async function createOrder() {
        if (busy || !savedCity || cityCode !== savedCity)
            return;

        setBusy(true);
        setError("");
        setSuccess("");

        try {
            const result = await apiJson("/api/orders", {
                method: "POST"
            });

            setCart(emptyCart);
            setSuccess(`Заказ ${result.orderId} успешно оформлен!` );
        } catch (err) {
            setError(err.message);
        } finally {
            setBusy(false);
        }
    }

    if (loading) {
        return <p>Загрузка корзины...</p>;
    }

    return (
        <section>
            <h1 className="mb-4">Корзина</h1>

            {error && (
                <div className="alert alert-danger">
                    {error}
                </div>
            )}

            {success && (
                <div className="alert alert-success">
                    {success}
                </div>
            )}

            {cart.items.length === 0 ? (
                <p>Корзина пуста.</p>
            ) : (
                <>
                    <div className="table-responsive">
                        <table className="table align-middle">
                            <thead>
                                <tr>
                                    <th>Товар</th>
                                    <th>Цена</th>
                                    <th>Количество</th>
                                    <th>Сумма</th>
                                    <th></th>
                                </tr>
                            </thead>

                            <tbody>
                                {cart.items.map(item => (
                                    <tr key={item.product.id}>
                                        <td>{item.product.name}</td>

                                        <td>
                                            {item.product.Price} ₽
                                        </td>

                                        <td>
                                            <div className="d-flex align-items-center gap-2">
                                                <button
                                                    className="btn btn-outline-secondary btn-sm"
                                                    disabled={busy || item.quantity <= 1}
                                                    onClick={() => updateQuantity(
                                                        item.product.id,
                                                        item.quantity - 1
                                                    )}
                                                >
                                                    -
                                                </button>

                                                <span>{item.quantity}</span>

                                                <button
                                                    className="btn btn-outline-secondary btn-sm"
                                                    disabled={busy || item.quantity >= 10}
                                                    onClick={() => updateQuantity(
                                                        item.product.id,
                                                        item.quantity + 1
                                                    )}
                                                >
                                                    +
                                                </button>
                                            </div>
                                        </td>

                                        <td>
                                            {item.quantity * item.product.price} ₽
                                        </td>

                                        <td>
                                            <button
                                                className="btn btn-outline-danger btn-sm"
                                                disabled={busy}
                                                onClick={() =>
                                                    removeItem(item.product.id)
                                                }
                                            >
                                                Удалить
                                            </button>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>

                    <h4 className="text-end">
                        Итого: {cart.totalPrice} ₽
                    </h4>

                    <div className="mt-4">
                        <label className="form-label">
                            Город доставки
                        </label>

                        <select
                            className="form-select mb-2"
                            value={cityCode}
                            onChange={event =>
                                setCityCode(event.target.value)
                            }
                        >
                            <option value="">Выберите город</option>

                            {cities.map(city => (
                                <option
                                    key={city.code}
                                    value={city.code}
                                >
                                    {city.name}
                                </option>
                            ))}
                        </select>

                        <button
                            className="btn btn-outline-primary"
                            disabled={
                                busy ||
                                !cityCode ||
                                cityCode === savedCity
                            }
                            onClick={saveCity}
                        >
                            Сохранить город
                        </button>
                    </div>

                    <button
                        className="btn btn-success mt-3"
                        disabled={
                            busy ||
                            !savedCity ||
                            cityCode !== savedCity
                        }
                        onClick={createOrder}
                    >
                        Оформить заказ
                    </button>
                </>
            )}

            <div className="mt-4">
                <Link to="/">Вернуться в каталог</Link>
            </div>
        </section>
    );
}