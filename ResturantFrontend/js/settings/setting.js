import { changePass } from "../api/users.js";
import { createPageHeader } from "../components/page-header.js";
import { createShowMessage } from "../components/show-message.js";
import { getStorage } from "../storage.js";
import { getRoleFromToken } from "../api/auth.js";

const UI = {
  pageHeader: document.getElementById("pageHeader"),
  profile: {
    username: document.getElementById("username"),
    role: document.getElementById("role"),
  },
  changePasswordForm: document.getElementById("changePasswordForm"),
  inputChangePass: {
    oldPass: document.getElementById("oldPass"),
    newPass: document.getElementById("pass"),
    confirmPass: document.getElementById("confirmPass"),
  },
  mess: document.getElementById("mess"),
};

UI.pageHeader.appendChild(
  createPageHeader(
    "Settings Screen",
    "../../assets/images/icons8-settings-200.png",
    "Settings",
  ),
);

function showMessage(mess, isSuccess) {
  UI.mess.style.display = "inline";
  UI.mess.replaceChildren(createShowMessage(mess, isSuccess));

  setTimeout(() => {
    UI.mess.style.display = "none";
  }, 3600);
}

function validatePassword(newPass, confirmPass) {
  if (newPass.length < 8) {
    showMessage("Password must be at least 8 characters long", false);
    return false;
  }
  if (newPass !== confirmPass) {
    showMessage("Passwords do not match", false);
    return false;
  }
  return true;
}

function clearInputFields() {
  UI.inputChangePass.oldPass.value = "";
  UI.inputChangePass.newPass.value = "";
  UI.inputChangePass.confirmPass.value = "";
}

function renderProfileInfo() {
  clearInputFields();
  const username = getStorage("username");
  const role = getRoleFromToken();
  UI.profile.username.textContent = username;
  UI.profile.role.textContent = role;
}

async function saveChangePassword() {
  const changePasswordForm = {
    oldPassword: UI.inputChangePass.oldPass.value.trim(),
    newPassword: UI.inputChangePass.newPass.value.trim(),
  };
  if (
    !validatePassword(
      changePasswordForm.newPassword,
      UI.inputChangePass.confirmPass.value.trim(),
    )
  ) {
    return;
  }

  try {
    const res = await changePass(changePasswordForm);
    showMessage("Password changed successfully", true);
    clearInputFields();
  } catch (error) {
    console.error(error);
    showMessage("Failed to change password", false);
  }
}

function boot() {
  renderProfileInfo();
}
boot();

UI.changePasswordForm.addEventListener("submit", (e) => {
  e.preventDefault();
  saveChangePassword();
});
