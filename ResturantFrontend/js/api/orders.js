import { apiFetch } from "./api.js";
const orderUrl = "/Orders";

export async function getOrders() {
  return await apiFetch(orderUrl + "/AllOrders");
}

export async function addNewOrder(order) {
  return await apiFetch(orderUrl + "/AddNewOrder", {
    method: "POST",
    body: JSON.stringify(order),
  });
}

export async function updateOrder(Id, order) {
  return await apiFetch(orderUrl + `/UpdateOrder/${Id}`, {
    method: "PUT",
    body: JSON.stringify(order),
  });
}

export async function deleteOrder(Id) {
  return await apiFetch(orderUrl + `/DeleteOrder/${Id}`, {
    method: "DELETE",
  });
}

export async function addItem(orderId, item) {
  return await apiFetch(orderUrl + `/${orderId}/AddNewItem`, {
    method: "POST",
    body: JSON.stringify(item),
  });
}

export async function deleteItem(OrderId, itemId) {
  return await apiFetch(orderUrl + `/${OrderId}/DeleteItem/${itemId}`, {
    method: "DELETE",
  });
}

export async function updateItem(OrderId, itemId, item) {
  return await apiFetch(orderUrl + `/${OrderId}/UpdateItem/${itemId}`, {
    method: "PUT",
    body: JSON.stringify(item),
  });
}

export async function cancelledOrder(Id) {
  return await apiFetch(orderUrl + `/CancelledOrder/${Id}`, {
    method: "PUT",
  });
}

export async function completedOrder(Id) {
  return await apiFetch(orderUrl + `/CompletedOrder/${Id}`, {
    method: "PUT",
  });
}

export async function cookingOrder(Id) {
  return await apiFetch(orderUrl + `/CookingOrder/${Id}`, {
    method: "PUT",
  });
}

export async function pendingOrder(Id) {
  return await apiFetch(orderUrl + `/PendingOrder/${Id}`, {
    method: "PUT",
  });
}
