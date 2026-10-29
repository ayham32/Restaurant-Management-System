import { apiFetch } from "./api.js";

export async function login(username, password) {
  return await apiFetch("/Auth/Login", {
    method: "post",
    body: JSON.stringify({ username, password }),
  });
}

export function getRoleFromToken() {
  const token = localStorage.getItem("token");
  if (!token) return null;

  try {
    const payloadBase64 = token.split(".")[1];
    const base64 = payloadBase64.replace(/-/g, "+").replace(/_/g, "/");
    const jsonPayload = decodeURIComponent(
      window
        .atob(base64)
        .split("")
        .map((c) => "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2))
        .join(""),
    );
    const decoded = JSON.parse(jsonPayload);

    return (
      decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ||
      decoded["role"] ||
      null
    );
  } catch (e) {
    return null;
  }
}

export function isTokenExpired() {
  const token = localStorage.getItem("token");
  if (!token) return true;

  try {
    const payloadBase64 = token.split(".")[1];
    const decoded = JSON.parse(
      atob(payloadBase64.replace(/-/g, "+").replace(/_/g, "/")),
    );
    return decoded.exp < Date.now() / 1000;
  } catch (e) {
    return true;
  }
}

export async function logout(username, refreshToken) {
  return await apiFetch("/Auth/Logout", {
    method: "post",
    body: JSON.stringify({ username, refreshToken }),
  });
}
