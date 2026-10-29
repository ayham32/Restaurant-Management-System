import {
  allCustomers,
  addNew,
  update,
  deleteCustomer,
} from "./api/customers.js";
import { createPageHeader } from "./components/page-header.js";
import { createSearch } from "./components/search.js";
import { createRecordsTable } from "./components/records-table.js";
import { createShowMessage } from "./components/show-message.js";

const UI = {
  pageHeader: document.getElementById("pageHeader"),
  searchContainer: document.getElementById("searchContainer"),
  btnAdd: document.getElementById("btnAdd"),
  tableBody: document.getElementById("tableBody"),
  records: document.getElementById("records"),
  addEditCustomer: document.getElementById("addEditCustomer"),
  title: document.getElementById("title"),
  addEditCustomerForm: document.getElementById("addEditCustomerForm"),
  input: {
    name: document.getElementById("name"),
    phone: document.getElementById("phone"),
    address: document.getElementById("address"),
  },
  mess: document.getElementById("mess"),
};

let Customers = [];
let filtered = [];
let mode = "Add";
let searchBy = "employeeId";
let searchValue = "";
let customerId = -1;

function showMessage(mess, isSuccess) {
  UI.mess.style.display = "inline";
  UI.mess.replaceChildren(createShowMessage(mess, isSuccess));

  setTimeout(() => {
    UI.mess.style.display = "none";
  }, 3600);
}

const searchOptions = [
  { value: "customerId", label: "Customer ID" },
  { value: "name", label: "Name" },
  { value: "phone", label: "Phone" },
  { value: "address", label: "Address" },
];

const search = createSearch(searchOptions, (result) => {
  searchBy = result.searchBy;
  searchValue = result.searchValue;
  render();
});

async function getCoustomers() {
  return await allCustomers();
}

function filterCustomers() {
  filtered = Customers.filter((customer) =>
    String(customer[searchBy])
      .toLowerCase()
      .includes(searchValue.toLowerCase()),
  );
}

function fillCustoemrsData() {
  UI.tableBody.innerHTML = "";
  filtered.map((c) => {
    const tr = document.createElement("tr");
    tr.innerHTML = `<td>${c.customerId}</td>
                  <td>${c.name}</td>
                  <td>${c.phone}</td>
                  <td>${c.address}</td>
                 <td><button data-id="${c.customerId}" class= "btnEdit">Edit</button>/
                 <button data-id="${c.customerId}" class="btnDelete">Delete</button>
                 </td>`;
    UI.tableBody.appendChild(tr);
  });
}

function fillRecord() {
  UI.records.replaceChildren(createRecordsTable(filtered.length));
}

function render() {
  filterCustomers();
  fillRecord();
  fillCustoemrsData();
}

async function boot() {
  try {
    Customers = await getCoustomers();

    if (!Customers) return;

    render();
  } catch (error) {
    console.error(error);
  }
}

boot();

UI.pageHeader.appendChild(
  createPageHeader(
    "Customers Management",
    "../../assets/images/OIP (2).jfif",
    "Customers",
  ),
);

UI.searchContainer.appendChild(search);

function restForm() {
  UI.input.name.value = "";
  UI.input.phone.value = "";
  UI.input.address.value = "";
}

function addNewCustomer() {
  restForm();
  mode = "Add";
  UI.addEditCustomer.showModal();
  UI.title.textContent = "Add New Person";
}

function editCustomer(Id) {
  const customer = Customers.find((c) => c.customerId === Id);
  if (!customer) return;
  mode = "Edit";
  UI.addEditCustomer.showModal();
  UI.input.name.value = customer.name;
  UI.input.phone.value = customer.phone;
  UI.input.address.value = customer.address;
}

UI.btnAdd.addEventListener("click", addNewCustomer);

UI.tableBody.addEventListener("click", async (event) => {
  const Id = Number(event.target.dataset.id);
  if (event.target.classList.contains("btnDelete")) {
    if (confirm("Are you sure? when you deleted you cannot retern it!")) {
      await deleteCustomer(Id);
      showMessage("Deleted done!");
      boot();
    }
  }
  if (event.target.classList.contains("btnEdit")) {
    customerId = Id;

    editCustomer(Id);
  }
});

async function Save() {
  const customer = {
    name: UI.input.name.value,
    phone: UI.input.phone.value,
    address: UI.input.address.value,
  };

  try {
    if (mode === "Add") {
      await addNew(customer);
      showMessage("Added done!");
    } else {
      await update(customerId, customer);
      showMessage("Updated done!");
    }

    UI.addEditCustomer.close();
    boot();
  } catch (e) {
    console.log(e);
    showMessage(e, false);
  }
}

UI.addEditCustomerForm.addEventListener("submit", (event) => {
  event.preventDefault();
  Save();
});
