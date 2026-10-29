import { GetAllPeople, addNew, edit, deletePerson } from "./api/people.js";
import { createPageHeader } from "./components/page-header.js";
import { createPagination } from "./components/pagination.js";
import { createRecordsTable } from "./components/records-table.js";
import { createShowMessage } from "./components/show-message.js";

const UI = {
  pageHeader: document.getElementById("pageHeader"),
  search: document.getElementById("search"),
  Name: document.getElementById("Name"),
  pagination: document.getElementById("pagination"),
  btnAdd: document.getElementById("btnAdd"),
  tableBody: document.getElementById("tableBody"),
  records: document.getElementById("records"),
  addEditPerson: document.getElementById("addEditPerson"),
  addEditPersonForm: document.getElementById("addEditPersonForm"),
  input: {
    firstName: document.getElementById("firstName"),
    lastName: document.getElementById("lastName"),
    nationalNo: document.getElementById("nationalNo"),
    phone: document.getElementById("phone"),
    gender: document.getElementById("gender"),
    dateOfBirth: document.getElementById("dateOfBirth"),
  },
  title: document.getElementById("title"),

  mess: document.getElementById("mess"),
};

let People = [];
let mode = "Add";
let personId = -1;
let searchName = "";
let Gender = null;
let pageNumber = 1;
let pageSize = 3;
let totalCount = 0;

async function getPeople() {
  return await GetAllPeople({
    search: searchName,
    Gender,
    pageNumber,
    pageSize,
  });
}

function fillRecord() {
  UI.records.replaceChildren(createRecordsTable(People.data.items.length));
}

function fillPeopleData() {
  UI.tableBody.innerHTML = "";
  People.data.items.map((p) => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
                  <td>${p.personId}</td>
                  <td>${p.firstName}</td>
                  <td>${p.lastName}</td>
                  <td>${p.nationalNo}</td>              
                  <td>${p.phone}</td>
                  <td>${p.gender ? "Female" : "Male"}</td>
                  <td>${p.dateOfBirth}</td>
                  <td>
                    <button data-id="${p.personId}" class="btnEdit">Edit</button>
                    <button data-id="${p.personId}" class="btnDelete">Delete</button>
                  </td>`;
    UI.tableBody.appendChild(tr);
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
  fillPeopleData();
  fillPagination();
}

async function boot() {
  try {
    People = await getPeople();
    console.log(People);

    if (!People) return;
    pageNumber = People.data.pageNumber;
    pageSize = People.data.pageSize;
    totalCount = People.data.totalCount;
    render();
  } catch (error) {
    console.error(error);
  }
}

UI.pageHeader.appendChild(
  createPageHeader(
    "People Management",
    "../../assets/images/employees.png",
    "People",
  ),
);

boot();

function restForm() {
  UI.input.firstName.value = "";
  UI.input.lastName.value = "";
  UI.input.nationalNo.value = "";
  UI.input.phone.value = "";
  UI.input.dateOfBirth.value = "";
  UI.input.gender.value = 0;
}

function addNewPerson() {
  restForm();
  UI.addEditPerson.showModal();
  UI.title.textContent = "Add New Person";
  mode = "Add";
}

function editEmployee(Id) {
  const person = People.data.items.find((p) => p.personId === Id);
  if (!person) return;
  UI.addEditPerson.showModal();
  UI.title.textContent = "Edit Person";
  mode = "Edit";
  UI.input.firstName.value = person.firstName;
  UI.input.lastName.value = person.lastName;
  UI.input.nationalNo.value = person.nationalNo;
  UI.input.phone.value = person.phone;
  UI.input.dateOfBirth.value = person.dateOfBirth;
  UI.input.gender.value = person.gender;
}

UI.btnAdd.addEventListener("click", addNewPerson);

function showMessage(mess, isSuccess) {
  UI.mess.style.display = "inline";
  UI.mess.replaceChildren(createShowMessage(mess, isSuccess));

  setTimeout(() => {
    UI.mess.style.display = "none";
  }, 3600);
}

async function Save() {
  const person = {
    firstName: UI.input.firstName.value,
    lastName: UI.input.lastName.value,
    nationalNo: UI.input.nationalNo.value,
    phone: UI.input.phone.value,
    dateOfBirth: UI.input.dateOfBirth.value,
    gender: Number(UI.input.gender.value),
  };
  try {
    if (mode === "Add") {
      await addNew(person);
      showMessage("Added done!");
    } else {
      await edit(personId, person);
      showMessage("Updated done!");
    }
    UI.addEditPerson.close();
    boot();
  } catch (e) {
    console.log(e);
    showMessage(e, false);
  }
}

UI.tableBody.addEventListener("click", async (event) => {
  const Id = Number(event.target.dataset.id);
  if (event.target.classList.contains("btnDelete")) {
    if (confirm("Are you sure? when you deleted you cannot retern it!")) {
      await deletePerson(Id);
      showMessage("Deleted done!");
      boot();
    }
  }
  if (event.target.classList.contains("btnEdit")) {
    personId = Id;
    editEmployee(Id);
  }
});

function searchGender() {
  const gender = document.querySelector('input[name="gender"]:checked').value;
  console.log(gender);

  if (gender === "all") {
    Gender = null;
  } else if (gender === "male") {
    Gender = 0;
  } else if (gender === "female") {
    Gender = 1;
  }
}

UI.search.addEventListener("click", () => {
  searchGender();
  searchName = UI.Name.value;
  pageNumber = 1;

  boot();
});

UI.addEditPersonForm.addEventListener("submit", (event) => {
  event.preventDefault();
  Save();
});
