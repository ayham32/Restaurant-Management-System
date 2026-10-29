import {
  getCategories,
  addCategory,
  updateCategory,
  deleteCategory,
} from "./api/categories.js";
import { createShowMessage } from "./components/show-message.js";
import { createPageHeader } from "./components/page-header.js";

const UI = {
  pageHeader: document.getElementById("pageHeader"),
  mess: document.getElementById("mess"),
  cardsBox: document.getElementById("cardsBox"),
  addNewCategory: document.getElementById("addNewCategory"),
  addCategoryForm: document.getElementById("addCategoryForm"),
  categoryName: document.getElementById("categoryName"),
  title: document.getElementById("title"),
};

let categories = [];
let selectedCategoryId = -1;
let mode = "Add";

UI.pageHeader.appendChild(createPageHeader("Categories Management"));

function addNewCategory() {
  UI.addNewCategory.showModal();
  UI.title.textContent = "Add New Category";
  mode = "Add";
  UI.categoryName.value = "";
}

function editCategory(Id) {
  const category = categories.find((c) => c.categoryId === Id);
  if (!category) return;

  UI.addNewCategory.showModal();
  UI.title.textContent = "Edit Category";
  mode = "Edit";
  UI.categoryName.value = category.categoryName;
}

async function allCategories() {
  return await getCategories();
}

function fillCategories() {
  UI.cardsBox.innerHTML = "";
  categories.forEach((category) => {
    const card = document.createElement("div");
    card.classList.add("card");
    card.innerHTML = `
      <h3>${category.categoryId}</h3>
      <p>${category.categoryName}</p>
      <button class="btnEdit" data-id="${category.categoryId}">Edit</button>
      <button class="btnDelete" data-id="${category.categoryId}">Delete</button>
    `;

    UI.cardsBox.appendChild(card);
  });

  const div = document.createElement("div");
  div.className = "card";
  const addBtn = document.createElement("button");
  addBtn.textContent = "+";
  addBtn.className = "add-card";
  addBtn.id = "btnAdd";
  addBtn.addEventListener("click", addNewCategory);
  div.appendChild(addBtn);
  UI.cardsBox.appendChild(div);
}

function render() {
  fillCategories();
}

async function boot() {
  try {
    categories = await allCategories();

    if (!categories) return;

    render();
  } catch (error) {
    console.error(error);
  }
}

boot();

function showMessage(mess, isSuccess) {
  UI.mess.style.display = "inline";
  UI.mess.replaceChildren(createShowMessage(mess, isSuccess));

  setTimeout(() => {
    UI.mess.style.display = "none";
  }, 3600);
}

async function Save() {
  const categoryName = UI.categoryName.value.trim();
  const category = {
    categoryName: categoryName,
  };

  try {
    if (mode === "Add") {
      await addCategory(category);
      showMessage("Added done!", true);
    } else {
      await updateCategory(selectedCategoryId, category);
      showMessage("Updated done!", true);
    }
    UI.addNewCategory.close();
    boot();
  } catch (error) {
    showMessage(error, false);
  }
}

UI.addCategoryForm.addEventListener("submit", (e) => {
  e.preventDefault();
  Save();
});

UI.cardsBox.addEventListener("click", async (event) => {
  const Id = Number(event.target.dataset.id);
  if (event.target.classList.contains("btnDelete")) {
    if (confirm("Are you sure? when you deleted you cannot retern it!")) {
      await deleteCategory(Id);
      showMessage("Deleted done!");
      boot();
    }
  }
  if (event.target.classList.contains("btnEdit")) {
    mode = "Edit";
    selectedCategoryId = Id;
    editCategory(Id);
  }
});
