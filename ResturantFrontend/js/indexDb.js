let db = null;

function openDB() {
  return new Promise((resolve, reject) => {
    const request = indexedDB.open("RestaurantDB", 1);
    request.onupgradeneeded = () => {
      const db = request.result;
      db.createObjectStore("cart", {
        keyPath: "productId",
      });
    };
    request.onsuccess = () => resolve(request.result);

    request.onerror = () =>
      reject(request.error || new Error("Failed to open IndexedDB"));
  });
}

function tx(storeName, mode = "readonly") {
  if (!db) throw new Error("Database is not initialized.");
  const transaction = db.transaction(storeName, mode);
  return transaction.objectStore(storeName);
}

function idbRequestToPromise(request) {
  return new Promise((resolve, reject) => {
    request.onsuccess = () => resolve(request.result);
    request.onerror = () =>
      reject(request.error || new Error("IndexedDB request failed"));
  });
}

export async function increase(id) {
  let store = tx("cart", "readwrite");
  let res = await idbRequestToPromise(store.get(id));
  res.quantity++;
  await idbRequestToPromise(store.put(res));
}

export async function remove(id) {
  let store = tx("cart", "readwrite");
  let item = await idbRequestToPromise(store.get(id));
  if (!item) throw Error("this product is not exist");

  await idbRequestToPromise(store.delete(id));
}

export async function decrease(id) {
  let store = tx("cart", "readwrite");
  let res = await idbRequestToPromise(store.get(id));
  res.quantity--;
  if (res.quantity > 0) await idbRequestToPromise(store.put(res));
  else await idbRequestToPromise(store.delete(id));
}

export async function addToCart(id) {
  let store = tx("cart", "readwrite");
  let item = { productId: id, quantity: 1 };
  (await idbRequestToPromise(store.get(id)))
    ? await increase(id)
    : await idbRequestToPromise(store.add(item));
}

export async function initDB() {
  if (!db) db = await openDB();
}

export async function getCartItems() {
  let store = tx("cart");
  return await idbRequestToPromise(store.getAll());
}
export async function clearAll() {
  let store = tx("cart", "readwrite");
  return idbRequestToPromise(store.clear());
}
