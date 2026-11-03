const form = document.getElementById("catalog-filters");
const productList = document.getElementById("product-list");

let currentRequest;

async function updateCatalog() {
    // Отменяем предыдущий запрос, если пользователь быстро изменил несколько фильтров
    currentRequest?.abort();
    currentRequest = new AbortController();

    // Собираем значения из HTML-формы
    const params = new URLSearchParams(new FormData(form)); // Автоматически собирает все значения из формы и превращает их в query-parameters (в cshtml задаем через asp-for и тд)

    try {
        const response = await fetch(
            `/Products/List?${params}`,
            { signal: currentRequest.signal }
        );

        if (!response.ok) {
            throw new Error(`HTTP ${response.status}`);
        }

        // Сервер возвращает HTML, а не JSON
        const html = await response.text();

        // Заменяем старые карточки новыми
        productList.innerHTML = html;

        // Сохраняем фильтры в адресной строке - чтобы можно было копировать или после нажатия f5 все осталось как было
        history.replaceState(
            null,
            "",
            `${location.pathname}?${params}`
        );
    } catch (error) {
        if (error.name !== "AbortError") {
            console.error(error);
            alert("Не удалось загрузить товары");
        }
    }
}

// Пользователь изменил категорию или сортировку
form.addEventListener("change", updateCatalog);

// Пользователь нажал кнопку "Применить"
form.addEventListener("submit", event => {
    event.preventDefault();
    updateCatalog();
});