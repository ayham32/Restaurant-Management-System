import { apiFetch } from "./api.js";

const customerUrl = "/Customers";

export async function allCustomers() {
  return await apiFetch(customerUrl + "/AllCustomers");
}

export async function addNew(customer) {
  return await apiFetch(customerUrl + "/AddNewCustomer", {
    method: "POST",
    body: JSON.stringify(customer),
  });
}

export async function update(Id, customer) {
  return await apiFetch(customerUrl + `/UpdateCustomer/${Id}`, {
    method: "PUT",
    body: JSON.stringify(customer),
  });
}

export async function deleteCustomer(Id) {
  return await apiFetch(customerUrl + `/DeleteCustomer/${Id}`, {
    method: "DELETE",
  });
}
