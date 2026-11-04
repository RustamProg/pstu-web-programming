export default function ProductCard({ product, onAdd, adding }) {
    return (
        <div className="col">
            <div className="card h-100">
                <div className="card-body d-flex flex-column">
                    <h5 className="card-title">
                        {product.name}
                    </h5>

                    <p className="text-muted">
                        {product.category}
                    </p>

                    <p className="card-text">
                        {product.description}
                    </p>

                    <p className="fw-bold">
                        {product.price.toLocaleString("ru-RU")} ₽
                    </p>

                    <button
                        type="button"
                        className="btn btn-primary mt-auto"
                        onClick={() => onAdd(product.id)}
                        disabled={adding}
                    >
                        Добавить в корзину
                    </button>
                </div>
            </div>
        </div>
    );
}

