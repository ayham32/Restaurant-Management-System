import { apiFetch } from "./api.js";
const categoryUrl = "/Categories";

export async function getCategories() {
  return await apiFetch(categoryUrl + "/AllCategories");
}

export async function addCategory(category) {
  return await apiFetch(categoryUrl + "/AddNewCategory", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(category),
  });
}

export async function updateCategory(Id, category) {
  return await apiFetch(categoryUrl + `/UpdateCategory/${Id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(category),
  });
}

export async function deleteCategory(Id) {
  return await apiFetch(categoryUrl + `/DeleteCategory/${Id}`, {
    method: "DELETE",
  });
}
