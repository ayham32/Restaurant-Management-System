import { apiFetch } from "./api.js";
const productUrl = "/Products";

export async function getAllProducts({
  category = "",
  search = "",
  isAvailable = null,
  pageNumber = 1,
  pageSize = 10,
} = {}) {
  const params = new URLSearchParams();
  if (category) params.append("category", category);
  if (search) params.append("search", search);
  if (isAvailable !== null) params.append("isAvailable", isAvailable);
  params.append("pageNumber", pageNumber);
  params.append("pageSize", pageSize);
  return await apiFetch(productUrl + `/AllProducts?${params.toString()}`);
}
export async function addNew(product) {
  const formData = new FormData();

  formData.append("Name", product.name);
  formData.append("Price", product.price);
  formData.append("Quantity", product.quantity);
  formData.append("CategoryId", product.categoryId);
  formData.append("PreparationTime", product.preparationTime);
  formData.append("IsAvailable", product.isAvailable);

  if (product.image) {
    formData.append("Image", product.image);
  }
  return await apiFetch(productUrl + "/AddNewProduct", {
    method: "POST",
    body: formData,
  });
}
export async function update(productId, product) {
  const formData = new FormData();

  formData.append("Name", product.name);
  formData.append("Price", product.price);
  formData.append("Quantity", product.quantity);
  formData.append("CategoryId", product.categoryId);
  formData.append("PreparationTime", product.preparationTime);
  formData.append("IsAvailable", product.isAvailable);

  if (product.image) {
    formData.append("Image", product.image);
  }

  return await apiFetch(productUrl + `/UpdateProduct/${productId}`, {
    method: "PUT",
    body: formData,
  });
}
export async function deleteProduct(productId) {
  return await apiFetch(productUrl + `/DeleteProduct/${productId}`, {
    method: "DELETE",
  });
}
