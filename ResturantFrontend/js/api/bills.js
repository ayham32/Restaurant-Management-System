import { apiFetch } from "./api.js";
const billUrl = "/Bills";

export async function getBills() {
  return await apiFetch(billUrl + "/AllBills");
}

export async function addNew(bill) {
  return await apiFetch(billUrl + "/AddNewBill", {
    method: "POST",
    body: JSON.stringify(bill),
  });
}
