import {
  getAllEmployees,
  getEmployee,
  deleteEmployee,
  addNew,
  update,
} from "./api/employees.js";
import { createPageHeader } from "./components/page-header.js";
import { createSearch } from "./components/search.js";
import { createPagination } from "./components/pagination.js";
import { createRecordsTable } from "./components/records-table.js";
import { createShowMessage } from "./components/show-message.js";

const UI = {
  pageHeader: document.getElementById("pageHeader"),
  searchContainer: document.getElementById("searchContainer"),
  pagination: document.getElementById("pagination"),
  btnAdd: document.getElementById("btnAdd"),
  tableBody: document.getElementById("tableBody"),
  records: document.getElementById("records"),
  addNewEmployee: document.getElementById("addNewEmployee"),
  title: document.getElementById("title"),
  newEmployee: {
    employeeForm: document.getElementById("addEmployeeForm"),
    userID: document.getElementById("userID"),
    labUserId: document.getElementById("labUserId"),
    managerID: document.getElementById("managerID"),
    Salary: document.getElementById("Salary"),
    StartTime: document.getElementById("StartTime"),
    EndTime: document.getElementById("EndTime"),
  },
  showUserDetails: document.getElementById("showUserDetails"),
  userInfo: {
    userId: document.getElementById("userId"),
    personId: document.getElementById("personId"),
    username: document.getElementById("username"),
    name: document.getElementById("name"),
    nationalNo: document.getElementById("nationalNo"),
    IsActive: document.getElementById("IsActive"),
  },
  showEmployeeDetails: document.getElementById("showEmployeeDetails"),
  employeeInfo: {
    EmployeeId: document.getElementById("EmployeeId"),
    UserId: document.getElementById("UserId"),
    Username: document.getElementById("Username"),
    Name: document.getElementById("Name"),
    isActive: document.getElementById("isActive"),
    managerBy: document.getElementById("managerBy"),
    salary: document.getElementById("salary"),
    startTime: document.getElementById("startTime"),
    endTime: document.getElementById("endTime"),
  },
  mess: document.getElementById("mess"),
};

let Employees = [];
let filtered = [];
let mode = "Add";
let employeeId = -1;
let pageNumber = 1;
let pageSize = 10;
let totalCount = 0;
let searchBy = "employeeId";
let searchValue = "";

const searchOptions = [
  { value: "employeeId", label: "Employee ID" },
  { value: "userId", label: "User ID" },
  { value: "username", label: "Username" },
  { value: "fullName", label: "Name" },
  { value: "managerBy", label: "Manager ID" },
  { value: "salary", label: "Salary" },
];

const searchFields = {
  employeeId: (e) => e.employeeId,
  userId: (e) => e.user?.userId,
  username: (e) => e.user?.username,
  fullName: (e) => e.user?.fullName,
  salary: (e) => e.salary,
  managerId: (e) => e.managerId,
};

const search = createSearch(searchOptions, (result) => {
  searchBy = result.searchBy;
  searchValue = result.searchValue;
  render();
});

async function getEmployees() {
  return await getAllEmployees(pageNumber, pageSize);
}

function fillRecord() {
  UI.records.replaceChildren(createRecordsTable(filtered.length));
}

function filterEmployees() {
  const getValue = searchFields[searchBy];

  filtered = Employees.items.filter((employee) =>
    String(getValue(employee) ?? "")
      .toLowerCase()
      .includes(searchValue.toLowerCase()),
  );
}

function fillEmployeeData() {
  UI.tableBody.innerHTML = "";
  filtered.map((e) => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
                  <td>${e.employeeId}</td>
                  <td>${e.user.userId}<button data-id="${e.user.userId}" class = "showDetailsUser">Show Details</button></td>
                  <td>${e.user.fullName}</td>
                  <td>${e.user.username}</td>              
                  <td>
                    ${e.managerBy ?? "No manager"}
                    ${
                      e.managerBy
                        ? `<button data-id="${e.managerBy}" class="showDetails">Show Details</button>`
                        : ""
                    }
                  </td>
                  <td>${e.salary}</td>
                  <td>${e.startTime}</td>
                  <td>${e.endTime}</td>
                  <td>
                    <button data-id="${e.employeeId}" class="btnEdit">Edit</button>
                    <button data-id="${e.employeeId}" class="btnDelete">Delete</button>
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
  filterEmployees();
  fillRecord();
  fillEmployeeData();
  fillPagination();
}

async function boot() {
  try {
    Employees = await getEmployees();
    console.log(Employees);

    if (!Employees) return;
    pageNumber = Employees.pageNumber;
    pageSize = Employees.pageSize;
    totalCount = Employees.totalCount;
    render();
  } catch (error) {
    console.error(error);
  }
}

UI.pageHeader.appendChild(
  createPageHeader(
    "Employees Management",
    "../../assets/images/employees.png",
    "Employees",
  ),
);

UI.searchContainer.appendChild(search);

boot();

async function showEmployeeInfo(Id) {
  const employee = await getEmployee(Id);

  if (!employee) return;

  UI.showEmployeeDetails.showModal();
  UI.employeeInfo.EmployeeId.textContent = Id;
  UI.employeeInfo.UserId.textContent = employee.user.userId;
  UI.employeeInfo.Username.textContent = employee.user.username;
  UI.employeeInfo.Name.textContent = employee.user.fullName;
  UI.employeeInfo.isActive.textContent = employee.user.isActive
    ? "Active"
    : "Inactive";
  UI.employeeInfo.managerBy.textContent = employee.managerBy ?? "No manager";
  UI.employeeInfo.salary.textContent = employee.salary;
  UI.employeeInfo.startTime.textContent = employee.startTime;
  UI.employeeInfo.endTime.textContent = employee.endTime;
}

function showUserInfo(Id) {
  const user = filtered.find((e) => e.user.userId === Id).user;

  if (!user) return;
  UI.showUserDetails.showModal();
  UI.userInfo.userId.textContent = Id;
  UI.userInfo.personId.textContent = user.personId;
  UI.userInfo.username.textContent = user.username;
  UI.userInfo.name.textContent = user.fullName;
  UI.userInfo.nationalNo.textContent = user.nationalNo;
  UI.userInfo.IsActive.textContent = user.isActive ? "Active" : "Inactive";
}

function restForm() {
  UI.newEmployee.userID.value = "";
  UI.newEmployee.managerID.value = "";
  UI.newEmployee.Salary.value = "";
  UI.newEmployee.StartTime.value = "";
  UI.newEmployee.EndTime.value = "";
}

function addNewEmployee() {
  restForm();
  UI.addNewEmployee.showModal();
  UI.title.textContent = "Add New Person";
  UI.newEmployee.labUserId.style.display = "inline";
  UI.newEmployee.userID.style.display = "inline";
  UI.newEmployee.userID.required = true;
}

function editEmployee(Id) {
  const employee = filtered.find((e) => e.employeeId === Id);
  if (!employee) return;

  UI.addNewEmployee.showModal();
  UI.newEmployee.labUserId.style.display = "none";
  UI.newEmployee.userID.style.display = "none";
  UI.newEmployee.userID.required = false;

  UI.title.textContent = "Edit Employee";
  UI.newEmployee.managerID.value = employee.managerBy ?? "";
  UI.newEmployee.Salary.value = employee.salary;
  UI.newEmployee.StartTime.value = employee.startTime;
  UI.newEmployee.EndTime.value = employee.endTime;
}

UI.btnAdd.addEventListener("click", () => {
  mode = "Add";
  addNewEmployee();
});

function showMessage(mess, isSuccess) {
  UI.mess.style.display = "inline";
  UI.mess.replaceChildren(createShowMessage(mess, isSuccess));

  setTimeout(() => {
    UI.mess.style.display = "none";
  }, 3600);
}

async function Save() {
  const userId = UI.newEmployee.userID.value;
  const manager = UI.newEmployee.managerID.value;
  const salary = UI.newEmployee.Salary.value;
  const start = UI.newEmployee.StartTime.value;
  const end = UI.newEmployee.EndTime.value;

  try {
    if (mode === "Add") {
      const newEmployee = {
        userId: Number(userId),
        managerId: manager ? Number(managerID) : null,
        salary: salary,
        startTime: start,
        endTime: end,
      };
      await addNew(newEmployee);
      showMessage("Added done!");
    } else {
      const updateEmployee = {
        managerId: manager ? Number(manager) : null,
        salary: salary,
        startTime: start,
        endTime: end,
      };
      await update(employeeId, updateEmployee);
      showMessage("Updated done!");
    }

    UI.addNewEmployee.close();
    boot();
  } catch (e) {
    console.log(e);
    showMessage(e, false);
  }
}

UI.addNewEmployee.addEventListener("submit", (e) => {
  e.preventDefault();
  Save();
});

UI.tableBody.addEventListener("click", async (event) => {
  const Id = Number(event.target.dataset.id);
  if (event.target.classList.contains("showDetails")) showEmployeeInfo(Id);
  if (event.target.classList.contains("showDetailsUser")) showUserInfo(Id);
  if (event.target.classList.contains("btnDelete")) {
    if (confirm("Are you sure? when you deleted you cannot retern it!")) {
      await deleteEmployee(Id);
      showMessage("Deleted done!");
      boot();
    }
  }
  if (event.target.classList.contains("btnEdit")) {
    mode = "Edit";
    employeeId = Id;
    editEmployee(Id);
  }
});
