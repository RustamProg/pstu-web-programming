export async function apiJson(url, options = {}) {
    const method = (options.method ?? "GET").toUpperCase();
    const headers = { ...options.headers };

    if (method !== "GET" && method !== "HEAD") {
        const tokenResponse = await fetch(
            "/api/security/antiforgery"
        );

        if (!tokenResponse.ok) {
            throw new Error("Не удалось получить CSRF-токен");
        }

        const { token } = await tokenResponse.json();

        headers["RequestVerificationToken"] = token;
    }

    const response = await fetch(url, {
        ...options,
        method,
        headers
    });

    const data = await response.json().catch(() => null);

    if (!response.ok) {
        throw new Error(data?.message ?? `Ошибка HTTP ${response.status}`);
    }

    return data;
}