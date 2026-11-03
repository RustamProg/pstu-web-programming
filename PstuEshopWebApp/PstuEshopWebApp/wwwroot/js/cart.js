(() => {
    const list = document.getElementById("product-list");
    const counter = document.getElementById("cart-count");
    const token = document.getElementById("cart-antiforgery-token").value;

    // Получаем количество товаров при открытии страницы
    async function loadCount() {
        const response = await fetch("/api/cart/summary");

        if (!response.ok)
            throw new Error(`HTTP ${response.status}`);

        const data = await response.json();
        counter.textContent = data.totalQuantity;
    }

    loadCount().catch(console.error);

    // Обрабатываем нажатия на кнопки товаров
    list.addEventListener("click", async event => {
        if (!(event.target instanceof Element))
            return;

        const button = event.target.closest(".js-add-to-cart");

        if (!button)
            return;

        button.disabled = true;

        try {
            const response = await fetch("/api/cart/items", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "RequestVerificationToken": token
                },
                body: JSON.stringify({
                    productId: Number(button.dataset.productId)
                })
            });

            if (!response.ok)
                throw new Error(`HTTP ${response.status}`);

            const data = await response.json();

            counter.textContent = data.totalQuantity;
        } catch (error) {
            console.error(error);
            alert("Не удалось добавить товар");
        } finally {
            button.disabled = false;
        }
    });
})();