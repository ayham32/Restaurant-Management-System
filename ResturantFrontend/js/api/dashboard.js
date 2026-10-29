import { apiFetch } from "./api.js";

let url = "/Dashboard";
export async function Statistics() {
  return await apiFetch(url + "/Statistics");
}

export async function RecentOrders() {
  return await apiFetch(url + "/RecentOrders");
}
