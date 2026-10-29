import { login, isTokenExpired, getRoleFromToken } from "../api/auth.js";
import { getStorage, setStorage } from "../storage.js";

const UI = {
  home: document.getElementById("btnHome"),
  loginInfo: document.getElementById("loginInfo"),
  username: document.getElementById("txtUsername"),
  pass: document.getElementById("txtPass"),
  remember: document.getElementById("cbRemember"),
  btnLogin: document.getElementById("btnLogin"),
  message: document.getElementById("ms"),
};

UI.home.addEventListener("click", () => {
  window.location.href = "../../pages/public/index.html";
});

document.addEventListener("DOMContentLoaded", () => {
  const isRemember = getStorage("remember");
  console.log(isRemember);

  const token = getStorage("token");
  console.log(token);

  if (isRemember)
    if (token) {
      const role = getRoleFromToken();
      if (role === "Admin")
        window.location.href = "../../pages/admin/dashboard.html";
      else if (role === "Employee")
        window.location.href = "../../pages/employee/dashboard.html";
    }
  UI.remember.checked = getStorage("remember");
});

UI.remember.addEventListener("change", () => {
  setStorage("remember", UI.remember.checked);
});

UI.loginInfo.addEventListener("submit", async (e) => {
  e.preventDefault();

  const username = UI.username.value.trim();
  const pass = UI.pass.value.trim();

  if (!username || !pass) {
    UI.message.textContent = "Username/Password invalid";
    return;
  }

  UI.message.textContent = "";

  try {
    const res = await login(username, pass);

    if (!res?.token) {
      UI.message.textContent = "Login failed";
      return;
    }

    setStorage("token", res.token);
    setStorage("username", username);
    setStorage("refreshToken", res.refreshToken);

    const role = getRoleFromToken();

    if (role === "Admin") {
      window.location.href = "../../pages/admin/dashboard.html";
    } else if (role === "Employee") {
      window.location.href = "../../pages/employee/dashboard.html";
    } else {
      UI.message.textContent = "User role not recognized";
    }
  } catch (e) {
    console.error(e);
    UI.message.textContent = "Username/Password invalid";
  }
});
