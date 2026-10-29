import { apiFetch } from "./api.js";

const employeeUrl = "/Employees";

export async function getAllEmployees(pageNumber = 1, pageSize = 10) {
  return await apiFetch(
    employeeUrl + `/AllEmployees?PageNumber=${pageNumber}&PageSize=${pageSize}`,
  );
}

export async function getEmployee(Id) {
  return await apiFetch(employeeUrl + `/Employee/${Id}`);
}

export async function deleteEmployee(Id) {
  return await apiFetch(employeeUrl + `/DeleteEmployee/${Id}`, {
    method: "DELETE",
  });
}

export async function addNew(newEmployee) {
  return await apiFetch(employeeUrl + "/AddNewEmployee", {
    method: "post",
    body: JSON.stringify(newEmployee),
  });
}

export async function update(Id, updateEmployee) {
  return await apiFetch(employeeUrl + `/UpdateEmployee/${Id}`, {
    method: "PUT",
    body: JSON.stringify(updateEmployee),
  });
}
