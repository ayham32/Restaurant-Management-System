const STORAGE_KEY = {
  THEME: "theme",
  TOKEN: "token",
  REMEMBER: "remember",
  USERNAME: "username",
  REFRESHTOKEN: "refreshToken",
};

export function setStorage(key, value) {
  localStorage.setItem(key, value);
}

export function getStorage(key) {
  return localStorage.getItem(key);
}

export function removeStorage(key) {
  localStorage.removeItem(key);
}
