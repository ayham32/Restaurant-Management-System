import {
  getAllUsers,
  addNewUser,
  deleteUser,
  getUserById,
  updateUser,
  getRoleId,
} from "./api/users.js";
import { getPersonById } from "./api/people.js";
import { createPageHeader } from "./components/page-header.js";
import { createRecordsTable } from "./components/records-table.js";
import { createPagination } from "./components/pagination.js";
import { createSearch } from "./components/search.js";
import { createShowMessage } from "./components/show-message.js";

const UI = {
  pageHeader: document.getElementById("pageHeader"),
  btnAdd: document.getElementById("btnAdd"),
  pagination: document.getElementById("pagination"),
  userBody: document.getElementById("userBody"),
  records: document.getElementById("records"),
  addNew: document.getElementById("addNewUser"),
  search: document.getElementById("btnSearch"),
  Username: document.getElementById("Username"),
  active: document.getElementById("active"),
  title: document.getElementById("title"),
  addUserForm: document.getElementById("addUserForm"),
  input: {
    personId: document.getElementById("personId"),
    username: document.getElementById("username"),
    pass: document.getElementById("pass"),
    confirmPass: document.getElementById("confirmPass"),
    isActive: document.getElementById("isActive"),
    role: document.getElementById("roles"),
    labConfirmPass: document.getElementById("labConfirmPass"),
    labPass: document.getElementById("labPass"),
  },
  save: document.getElementById("save"),
  mess: document.getElementById("mess"),
  showPersonDetails: document.getElementById("showPersonDetails"),
  PersonId: document.getElementById("PersonId"),
  firstName: document.getElementById("firstName"),
  lastName: document.getElementById("lastName"),
  NationalNo: document.getElementById("NationalNo"),
  gendor: document.getElementById("gendor"),
  phone: document.getElementById("phone"),
  dateOfBirth: document.getElementById("dateOfBirth"),
};

let Users = [];
let mode = "Add";
let userId = -1;
let pageNumber = 1;
let pageSize = 2;
let totalCount = 0;
let searchByUsername = "";
let active = null;

async function getUsers() {
  return await getAllUsers({
    username: searchByUsername,
    active,
    pageNumber,
    pageSize,
  });
}

function fillRecord() {
  UI.records.replaceChildren(createRecordsTable(Users.data.items.length));
}

function fillUserData() {
  UI.userBody.innerHTML = "";
  Users.data.items.forEach((u) => {
    const tr = document.createElement("tr");
    tr.innerHTML = `<td>${u.userId}</td>
                  <td>${u.personId}<button data-id="${u.personId}" class = "showDetails">Show Details</button></td>
                  <td>${u.fullName}</td>
                  <td>${u.nationalNo}</td>
                  <td>${u.username}</td>
                  <td>${u.isActive ? "Active" : "inActive"}</td>
                  <td>
                  <button data-id="${u.userId}" class= "btnEdit">Edit</button>/
                  <button data-id="${u.userId}" class="btnDelete">Delete</button>
                  </td>`;
    UI.userBody.appendChild(tr);
  });
}

function fillPagination() {
  UI.pagination.innerHTML = "";

  const pagination = createPagination({
    pageNumber,
    totalCount,
    pageSize,

    onPageChange: async (newPage) => {
      pageNumber = newPage;
      await boot();
    },
  });

  UI.pagination.appendChild(pagination);
}

function render() {
  fillRecord();
  fillUserData();
  fillPagination();
}

async function boot() {
  try {
    Users = await getUsers();

    if (!Users) return;
    console.log(Users);

    pageNumber = Users.data.pageNumber;
    pageSize = Users.data.pageSize;
    totalCount = Users.data.totalCount;
    render();
  } catch (error) {
    console.error(error);
  }
}

function addUser() {
  restForm();
  UI.addNew.showModal();
  UI.title.textContent = "Add New Person";
  UI.input.pass.style.display = "inline";
  UI.input.confirmPass.style.display = "inline";
  UI.input.pass.required = true;
  UI.input.confirmPass.required = true;
  UI.input.labConfirmPass.style.display = "inline";
  UI.input.labPass.style.display = "inline";
}

UI.btnAdd.addEventListener("click", () => {
  mode = "Add";
  addUser();
});

function isMatch() {
  return UI.input.pass.value === UI.input.confirmPass.value;
}

function restForm() {
  UI.input.personId.value = "";
  UI.input.username.value = "";
  UI.input.pass.value = "";
  UI.input.confirmPass.value = "";
  UI.input.isActive.checked = true;
  UI.mess.textContent = "";
  UI.save.disabled = false;
}

function ShowMessage(mess, isSuccess) {
  UI.mess.style.display = "inline";
  UI.mess.replaceChildren(createShowMessage(mess, isSuccess));

  setTimeout(() => {
    UI.mess.style.display = "none";
  }, 3600);
}

async function Save() {
  const PersonId = UI.input.personId.value;
  const Username = UI.input.username.value;
  const IaActive = UI.input.isActive.checked;
  const Role = UI.input.role.value;

  try {
    if (mode === "Add") {
      const Password = UI.input.pass.value;
      await addNewUser(PersonId, Username, Password, IaActive, Role);
      ShowMessage("User added successfully!", true);
    } else {
      await updateUser(userId, PersonId, Username, IaActive, Role);
      ShowMessage("User updated successfully!", true);
    }

    UI.save.disabled = true;
    UI.addNew.close();
    boot();
  } catch (e) {
    console.log(e);

    ShowMessage(e.message, false);
  }
}

UI.addUserForm.addEventListener("submit", (e) => {
  e.preventDefault();

  if (!isMatch()) {
    ShowMessage("The password doesnt match", false);
    return;
  }
  Save();
});

async function ShowPersonDetails(Id) {
  const person = await getPersonById(Id);
  if (!person) {
    return;
  }
  UI.showPersonDetails.showModal();
  UI.PersonId.textContent = person.personId;
  UI.firstName.textContent = person.firstName;
  UI.lastName.textContent = person.lastName;
  UI.NationalNo.textContent = person.nationalNo;
  UI.gendor.textContent = !person.gendor ? "Male" : "Female";
  UI.phone.textContent = person.phone;
  UI.dateOfBirth.textContent = person.dateOfBirth;
}

async function editUser(Id) {
  const user = await getUserById(Id);
  if (!user) return;

  const userRole = await getRoleId(Id);
  if (!userRole) return;
  UI.save.disabled = false;

  UI.addNew.showModal();
  UI.title.textContent = "Edit User";
  UI.input.personId.value = user.personId;
  UI.input.username.value = user.username;
  UI.input.isActive.checked = user.isActive;
  UI.input.role.value = userRole[0].roleId ?? 2;
  UI.input.pass.style.display = "none";
  UI.input.confirmPass.style.display = "none";
  UI.input.pass.required = false;
  UI.input.confirmPass.required = false;
  UI.input.labConfirmPass.style.display = "none";
  UI.input.labPass.style.display = "none";
}

UI.userBody.addEventListener("click", async (event) => {
  const Id = Number(event.target.dataset.id);
  if (event.target.classList.contains("showDetails")) ShowPersonDetails(Id);
  if (event.target.classList.contains("btnDelete")) {
    if (confirm("Are you sure? when you deleted you cannot retern it!")) {
      await deleteUser(Id);
      boot();
    }
  }
  if (event.target.classList.contains("btnEdit")) {
    mode = "Edit";
    userId = Id;

    await editUser(Id);
  }
});

boot();

UI.pageHeader.appendChild(
  createPageHeader("Users Management", "../../assets/images/team.png", "Users"),
);
UI.search.addEventListener("click", () => {
  searchByUsername = UI.Username.value;
  active = UI.active.value;
  pageNumber = 1;

  boot();
});
