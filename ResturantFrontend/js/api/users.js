import { apiFetch } from "./api.js";

const userUrl = "/Users";

export async function getAllUsers({
  username = "",
  isActive = null,
  pageNumber = 1,
  pageSize = 10,
} = {}) {
  const params = new URLSearchParams();
  if (username) params.append("UserName", username);

  if (isActive !== null) params.append("isActive", isActive);
  params.append("pageNumber", pageNumber);
  params.append("pageSize", pageSize);
  return await apiFetch(userUrl + `/GetAllUsers?${params.toString()}`);
}

export async function getUserByUsername(username) {
  return await apiFetch(userUrl + `/GetUserWithUsername/${username}`);
}

export async function getUserById(Id) {
  return await apiFetch(userUrl + `/GetUserWithId/${Id}`);
}

export async function addNewUser(personId, username, pass, isActive, roleId) {
  return await apiFetch(userUrl + "/AddNewUser", {
    method: "post",
    body: JSON.stringify({ personId, username, pass, isActive, roleId }),
  });
}

export async function isUserExistByUsername(username) {
  return await apiFetch(userUrl + `/isUserExistsWithId/${username}`);
}

export async function deleteUser(id) {
  return await apiFetch(userUrl + `/DeleteUser/${id}`, {
    method: "DELETE",
  });
}
export async function updateUser(id, personId, username, isActive, roleId) {
  return await apiFetch(userUrl + `/UpdateUser/${id}`, {
    method: "PUT",
    body: JSON.stringify({ personId, username, isActive, roleId }),
  });
}
export async function getRoleId(userId) {
  return await apiFetch(userUrl + `/Role/${userId}`);
}
export async function changePass(changePassDto) {
  return await apiFetch(userUrl + `/ChangePassword`, {
    method: "PUT",
    body: JSON.stringify(changePassDto),
  });
}
