// --- apiHelper.js ---
// Helper gọi API chung (GET/POST/PUT/DELETE)
async function callApi(url, method = "GET", data = null) {
    const options = { method, headers: {} };
    if (data) {
        options.headers['Content-Type'] = 'application/json';
        options.body = JSON.stringify(data);
    }
    const res = await fetch(url, options);
    if (!res.ok) throw new Error(`Lỗi API: ${res.status}`);
    return method === "GET" ? await res.json() : true;
}
