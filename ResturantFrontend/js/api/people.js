import { apiFetch } from "./api.js";

export async function GetAllPeople({
  search = "",
  Gender = null,
  pageNumber = 1,
  pageSize = 10,
} = {}) {
  const params = new URLSearchParams();
  if (search) params.append("search", search);
  if (Gender !== null) params.append("Gender", Gender);
  params.append("pageNumber", pageNumber);
  params.append("pageSize", pageSize);
  return apiFetch(`/People/AllPeople?${params.toString()}`);
}

export async function getPersonById(id) {
  return apiFetch(`/People/GetPersonWithId/${id}`);
}

export async function addNew(person) {
  return await apiFetch("/People/AddNewPerson", {
    method: "POST",
    body: JSON.stringify(person),
  });
}
export async function edit(Id, person) {
  return await apiFetch(`/People/UpdatePerson/${Id}`, {
    method: "PUT",
    body: JSON.stringify(person),
  });
}

export async function deletePerson(Id) {
  return await apiFetch(`/People/DeletePerson/${Id}`, {
    method: "DELETE",
  });
}
