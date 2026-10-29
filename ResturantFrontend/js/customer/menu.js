import { getAllProducts } from "../api/products.js";
import { SERVER_URL } from "../api/api.js";
import { initDB, addToCart } from "../indexDb.js";
import { createPagination } from "../components/pagination.js";

let UI = {
  count: document.getElementById("countProducts"),
  All: document.getElementById("btnAll"),
  Foods: document.getElementById("btnFoods"),
  Drinks: document.getElementById("btnDrinks"),
  Sweet: document.getElementById("btnSweets"),
  Search: document.getElementById("search"),
  pagination: document.getElementById("pagination"),
  listProduct: document.getElementById("listProductId"),
  toast: document.getElementById("toast"),
};

function showToast(message) {
  UI.toast.textContent = message;
  UI.toast.style.display = "block";
  setTimeout(() => (UI.toast.style.display = "none"), 2200);
}

let products = [];
let allProducts = [];
let pageNumber = 1;
let pageSize = 3;
let totalCount = 0;

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

async function loadProducts(categroy) {
  try {
    allProducts = products.data.items.map((p) => ({
      id: p.productId,
      name: p.name,
      price: p.price,
      quantity: p.quantity,
      categoryName: p.categoryName,
      preparationTime: p.preparationTime,
      isAvailable: p.isAvailable,
      createdAt: p.createdAt,
      imageUrl: SERVER_URL + p.imageUrl,
    }));

    searchProduct(categroy);
  } catch (e) {
    UI.listProduct.innerHTML = `<div class='muted'>${e.message}</div>`;
  }
}

let currentFilter = "";
function searchProduct(category = currentFilter) {
  currentFilter = category;

  const searchValue = UI.Search.value.trim().toLowerCase();

  const filtered = currentFilter
    ? allProducts.filter(
        (p) => p.categoryName.toLowerCase() === currentFilter.toLowerCase(),
      )
    : allProducts;

  const result = searchValue
    ? filtered.filter((p) => p.name.toLowerCase().includes(searchValue))
    : filtered;

  UI.count.textContent = `${result.length} shown`;

  UI.listProduct.innerHTML = result
    .map(
      (p) => `
      <div class="menu-item">
          <img src="${p.imageUrl}" alt="">
          <div class="menu-item-content">
              <h3>${p.name}</h3>
              <p class="price">Price: $${p.price}</p>
              <p>Preparation Time: ${p.preparationTime}m</p>
              </div>
          <button class="add-cart" data-id ="${p.id}">Add to Basket</button>
      </div>
  `,
    )
    .join("");

  if (result.length === 0) {
    UI.listProduct.innerHTML = "<div class='muted'>No products</div>";
  }
  fillPagination();
}

function render(category = "") {
  fillPagination();
  loadProducts(category);
}

UI.listProduct.addEventListener("click", (event) => {
  if (event.target.classList.contains("add-cart")) {
    addToCart(Number(event.target.dataset.id)).then(
      showToast("✅ Product added"),
    );
  }
});

UI.Search.addEventListener("input", () => render());
UI.Foods.addEventListener("click", () => render("food"));
UI.Drinks.addEventListener("click", () => render("drinks"));
UI.Sweet.addEventListener("click", () => render("sweets"));
UI.All.addEventListener("click", () => render(""));

async function boot() {
  try {
    await initDB();
    products = await getAllProducts({
      category: null,
      search: "",
      isAvailable: null,
      pageNumber,
      pageSize,
    });
    console.log(products);

    pageNumber = products.data.pageNumber;
    pageSize = products.data.pageSize;
    totalCount = products.data.totalCount;
    render();
  } catch (e) {
    showToast(`❌ Failed to initialize app: ${e.message}`);
  }
}
boot();
