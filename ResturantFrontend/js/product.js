import {
  getAllProducts,
  addNew,
  update,
  deleteProduct,
} from "./api/products.js";
import { createPageHeader } from "./components/page-header.js";
import { createShowMessage } from "./components/show-message.js";
import { createPagination } from "./components/pagination.js";
import { SERVER_URL } from "./api/api.js";
import { getCategories } from "./api/categories.js";
const UI = {
  pageHeader: document.getElementById("pageHeader"),
  mess: document.getElementById("mess"),
  searchBox: document.getElementById("searchBox"),
  pagination: document.getElementById("pagination"),
  btnAdd: document.getElementById("btnAdd"),
  cardsBox: document.getElementById("cardsBox"),
  addEditProduct: document.getElementById("addEditProduct"),
  addEditProductForm: document.getElementById("addEditProductForm"),
  title: document.getElementById("title"),
  categoryList: document.getElementById("categoryList"),
  Name: document.getElementById("Name"),
  available: document.getElementById("available"),
  search: document.getElementById("btnSearch"),
  input: {
    name: document.getElementById("name"),
    imagePreview: document.getElementById("imagePreview"),
    image: document.getElementById("imageUrl"),
    price: document.getElementById("price"),
    quantity: document.getElementById("quantity"),
    categoryName: document.getElementById("caategoryId"),
    preparationTime: document.getElementById("preparationTime"),
    isAvailable: document.getElementById("isAvailable"),
  },
};

let mode = "Add";
let products = [];
let pageNumber = 1;
let pageSize = 2;
let totalCount = 0;
let isAvailable = null;
let category = "";
let searchName = "";
let selectedProductId = null;

UI.pageHeader.appendChild(
  createPageHeader(
    "Products Management",
    "../../assets/images/dairy-products.png",
    "Products",
  ),
);

let categories = [];

async function fillCtegoriesFiltered() {
  categories = await getCategories();

  UI.categoryList.innerHTML = "";

  const oppAll = document.createElement("option");
  oppAll.value = "";
  oppAll.textContent = "All";
  UI.categoryList.appendChild(oppAll);

  categories.forEach((category) => {
    const option = document.createElement("option");

    option.value = category.categoryName;
    option.textContent = category.categoryName;

    UI.categoryList.appendChild(option);
  });
}

async function fillCategories() {
  if (!categories) categories = await getCategories();
  UI.input.categoryName.innerHTML = "";

  categories.forEach((category) => {
    const option = document.createElement("option");

    option.value = category.categoryId;
    option.textContent = category.categoryName;

    UI.input.categoryName.appendChild(option);
  });
}

async function getProducts() {
  return await getAllProducts({
    category,
    search: searchName,
    isAvailable,
    pageNumber,
    pageSize,
  });
}

function addNewProduct() {
  UI.addEditProduct.showModal();
  fillCategories();
  mode = "Add";
  UI.title.textContent = "Add New Product";
  UI.input.name.value = "";
  UI.input.price.value = "";
  UI.input.quantity.value = "";
  UI.input.categoryName.value = 1;
  UI.input.preparationTime.value = "";
  UI.input.isAvailable.checked = true;
  UI.input.imagePreview.src = "";
  UI.input.image.value = "";
}

UI.btnAdd.addEventListener("click", addNewProduct);

function editProduct(productId) {
  const product = products.data.items.find((p) => p.productId === productId);
  if (!product) return;
  UI.addEditProduct.showModal();
  mode = "Edit";
  fillCategories();
  const categoryId = categories.find(
    (c) => c.categoryName === product.categoryName,
  )?.categoryId;
  UI.title.textContent = "Edit Product";
  UI.input.name.value = product.name;
  UI.input.price.value = product.price;
  UI.input.quantity.value = product.quantity;
  UI.input.categoryName.value = categoryId || "";
  UI.input.preparationTime.value = product.preparationTime;
  UI.input.isAvailable.checked = product.isAvailable;
  UI.input.imagePreview.src = SERVER_URL + product.imageUrl || "";
  UI.input.image.value = product.imageUrl;
}

function showMessage(mess, isSuccess) {
  UI.mess.style.display = "inline";
  UI.mess.replaceChildren(createShowMessage(mess, isSuccess));

  setTimeout(() => {
    UI.mess.style.display = "none";
  }, 3600);
}

async function Save() {
  const product = {
    name: UI.input.name.value,
    price: parseFloat(UI.input.price.value),
    quantity: parseInt(UI.input.quantity.value),
    categoryId: UI.input.categoryName.value || "",
    preparationTime: parseInt(UI.input.preparationTime.value),
    isAvailable: UI.input.isAvailable.checked,
    image: UI.input.image.files[0],
  };

  try {
    if (mode === "Add") {
      await addNew(product);
      showMessage("Product added successfully", true);
    } else {
      await update(selectedProductId, product);
      showMessage("Product updated successfully", true);
    }
    UI.addEditProduct.close();
    boot();
  } catch (error) {
    console.error(error);
    showMessage(error.message, false);
  }
}

function fillProducts() {
  UI.cardsBox.innerHTML = "";
  products.data.items.forEach((p) => {
    const card = document.createElement("div");
    card.classList.add("card");
    card.innerHTML = `
      <h3>${p.productId}</h3>
      <p><img src="${SERVER_URL + p.imageUrl}" alt="${p.name}" /></p>
      <p>${p.name}</p>
      <p>Price: $${p.price.toFixed(2)}</p>
      <p>Quantity: ${p.quantity}</p>
      <p>Category: ${p.categoryName}</p>
      <p>Preparation Time: ${p.preparationTime} mins</p>
      <p>${p.isAvailable ? "Available" : "Not Available"}</p>
      <p>Created At: ${p.createdAt}</p>
       <button class="btnEdit" data-id="${p.productId}">Edit</button>
      <button class="btnDelete" data-id="${p.productId}">Delete</button>
    `;
    UI.cardsBox.appendChild(card);
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
  fillCtegoriesFiltered();
  fillProducts();
  fillPagination();
}

async function boot() {
  try {
    products = await getProducts();
    pageNumber = products.data.pageNumber;
    pageSize = products.data.pageSize;
    totalCount = products.data.totalCount;
    console.log(totalCount);

    render();
  } catch (error) {
    console.error(error);
    showMessage(error.message, false);
  }
}

boot();

UI.addEditProductForm.addEventListener("submit", (e) => {
  e.preventDefault();
  Save();
});

UI.cardsBox.addEventListener("click", async (event) => {
  const Id = Number(event.target.dataset.id);
  if (event.target.classList.contains("btnDelete")) {
    if (confirm("Are you sure? when you deleted you cannot retern it!")) {
      await deleteProduct(Id);
      showMessage("Deleted done!");
      boot();
    }
  }
  if (event.target.classList.contains("btnEdit")) {
    selectedProductId = Id;
    editProduct(Id);
  }
});

UI.input.image.addEventListener("change", (event) => {
  const file = event.target.files[0];
  if (file) {
    UI.input.imagePreview.src = URL.createObjectURL(file);
  }
});

UI.search.addEventListener("click", () => {
  category = UI.categoryList.value;
  searchName = UI.Name.value;
  isAvailable = UI.available.value;
  pageNumber = 1;

  boot();
});
