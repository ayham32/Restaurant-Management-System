import { setStorage, getStorage, removeStorage } from "./storage.js";
import { getUserByUsername } from "./api/users.js";
import { initApp } from "./layout.js";
import { getRoleFromToken, isTokenExpired } from "./api/auth.js";
import { logout } from "./api/auth.js";

const body = document.body;

function updateButtonText(btnElement) {
  if (!btnElement) return;

  btnElement.textContent = body.classList.contains("dark")
    ? "Light Mode ☀️"
    : "Dark Mode 🌙";
}

function toggleMode(targetBtn) {
  const isDark = body.classList.toggle("dark");
  setStorage("theme", isDark ? "dark" : "light");

  updateButtonText(targetBtn);
}

async function Logout() {
  const username = getStorage("username");
  const refreshToken = getStorage("refreshToken");
  try {
    if (refreshToken) await logout(username, refreshToken);
  } catch (error) {
    console.error("Error while logging out of the server: " + error);
  } finally {
    redirectToLogin();
  }
}

document.addEventListener("click", (event) => {
  const btnToggle = event.target.closest(".btn-mode");
  if (btnToggle) {
    toggleMode(btnToggle);
  }
  const btnLogout = event.target.closest(".btnLogout");
  if (btnLogout) Logout();
});

function theme() {
  const savedTheme = getStorage("theme");
  if (savedTheme === "dark") {
    body.classList.add("dark");
  } else {
    body.classList.remove("dark");
  }

  const switchBtn =
    document.querySelector("#switchMode .btn-mode") ||
    document.getElementById("switchMode");
  updateButtonText(switchBtn);
}

async function updateSidebarName(username) {
  const nameElement = document.querySelector(".your-name");
  if (!nameElement) return;

  try {
    const user = await getUserByUsername(username);
    if (!user) return;

    nameElement.textContent = user.fullName;
  } catch (error) {
    console.error("not found name", error);
  }
}

function renderSidebarByRole() {
  const userRole = getRoleFromToken();
  if (!userRole) return;

  if (userRole === "Employee") {
    const dashboardLink = document.querySelector(
      '.side-bar a[href*="dashboard.html"]',
    );
    if (dashboardLink) {
      dashboardLink.href = "../../pages/employee/dashboard.html";
    }

    const employeesLink = document.querySelector(
      '.side-bar a[href*="employee.html"]',
    );
    const usersLink = document.querySelector('.side-bar a[href*="user.html"]');

    if (employeesLink) employeesLink.remove();
    if (usersLink) usersLink.remove();
  }
}

function checkPageGuard() {
  const requiredRole = document.body.dataset.role;

  if (!requiredRole) return;

  const token = getStorage("token");

  if (!token) {
    return redirectToLogin();
  }

  try {
    const isExpired = isTokenExpired();
    const userRole = getRoleFromToken();
    const allowedRoles = requiredRole.split(",").map((role) => role.trim());
    const isAllowed = allowedRoles.includes(userRole);
    if (isExpired || !isAllowed) {
      return redirectToLogin();
    }

    body.style.display = "block";
  } catch (e) {
    redirectToLogin();
  }
}

function redirectToLogin() {
  removeStorage("token");
  removeStorage("username");
  removeStorage("refreshToken");
  window.location.replace("../../pages/auth/login.html");
}

async function init() {
  try {
    checkPageGuard();
    await initApp();
    renderSidebarByRole();
    theme();

    const username = getStorage("username");
    if (username) {
      await updateSidebarName(username);
    }
  } catch (error) {
    console.error("error in system initilzion", error);
  }
}

document.addEventListener("click", (e) => {
  if (e.target.classList.contains("close-btn"))
    e.target.closest("dialog").close();
});
init();
