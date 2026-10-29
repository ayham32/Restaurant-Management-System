import { getStorage, setStorage, removeStorage } from "../storage.js";
const BASE_URL = "https://localhost:7115/api";
export const SERVER_URL = "https://localhost:7115";

async function handleRefreshToken() {
  const refreshToken = getStorage("refreshToken");
  const username = getStorage("username");

  if (!refreshToken || !username) {
    throw new Error("Refresh token or username is missing.");
  }
  const response = await fetch(BASE_URL + "/Auth/Refresh", {
    method: "post",
    headers: { "Content-Type": "application/json" },
    credentials: "include",
    body: JSON.stringify({
      refreshToken,
      username,
    }),
  });

  if (!response.ok) {
    throw new Error("Failed to refresh token");
  }

  const data = await response.json();

  setStorage("token", data.token);
  setStorage("refreshToken", data.refreshToken);

  return data.token;
}

export async function apiFetch(endpoint, options = {}, isRetry = false) {
  const token = getStorage("token");
  const headers = {
    ...(token ? { Authorization: `Bearer ${token}` } : {}),
    ...(options.headers || {}),
  };

  if (!(options.body instanceof FormData)) {
    headers["Content-Type"] = "application/json";
  }

  const response = await fetch(BASE_URL + endpoint, {
    ...options,
    headers,
  });
  if (response.status === 401 && !isRetry && endpoint !== "/Auth/Refresh") {
    try {
      const newAccessToken = await handleRefreshToken();
      return await apiFetch(
        endpoint,
        {
          ...options,
          headers: {
            ...options.headers,
            Authorization: `Bearer ${newAccessToken}`,
          },
        },
        true,
      );
    } catch (error) {
      removeStorage("token");
      window.location.href = "../../auth/login.html";
      throw new Error("The session has ended. Please log in again.");
    }
  }
  if (!response.ok) {
    const errorText = await response.text();

    let errorData;

    try {
      errorData = JSON.parse(errorText);
    } catch {
      throw new Error(errorText || `HTTP Error ${response.status}`);
    }

    if (errorData.errors) {
      const messages = Object.values(errorData.errors).flat();

      throw new Error(messages.join("\n"));
    }

    throw new Error(
      errorData.title || errorData.detail || `HTTP Error ${response.status}`,
    );
  }

  const contentType = response.headers.get("content-type");

  if (contentType?.includes("application/json")) {
    return await response.json();
  }

  return await response.text();
}
