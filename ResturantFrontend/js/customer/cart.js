import { getAllProducts } from "../api/products.js";
import { SERVER_URL } from "../api/api.js";
import {
  initDB,
  getCartItems,
  clearAll,
  decrease,
  increase,
  remove,
} from "../indexDb.js";
let UI = {
  backShop: document.getElementById("btnBackShop"),
  basketBody: document.getElementById("basketBody"),
  emptyMs: document.getElementById("emptyMs"),
  clearAll: document.getElementById("btnClearAll"),
  totalAmount: document.getElementById("totalAmount"),
  totalPreparation: document.getElementById("totalPreparation"),
  dialog: document.querySelector("#productDialog"),
  dialogTitle: document.getElementById("dialogTitle"),
  dialogImage: document.getElementById("dialogImage"),
  dialogPrice: document.getElementById("dialogPrice"),
  dialogDescription: document.getElementById("dialogDescription"),
  close: document.getElementById("close-btn"),
};

let items = [];
let products = [];

function showItemCart() {
  let totalPrice = 0;
  let totalPreparation = 0;
  items.forEach((item) => {
    const p = products.data.items.find((id) => id.productId === item.productId);
    if (!p) return;
    const tr = document.createElement("tr");
    tr.innerHTML = `
              <td>${p.name}</td>
              <td>
              <button data-id = "${p.productId}" class = "small-btn increase" >+</button>
              ${item.quantity}
              <button data-id ="${p.productId}"class = "small-btn decrease">-</button>
              </td>
              <td>$${p.price.toFixed(2)}</td>
              <td>$${(p.price * item.quantity).toFixed(2)}</td>
          
             <td>
             <button data-id = "${p.productId}" class="show-more" >Show More!</button>
              <button data-id = "${p.productId}" class="btn-danger delete" >Delete</button>
              
              </td>
              `;
    UI.basketBody.appendChild(tr);
    totalPrice += p.price * item.quantity;
    totalPreparation += p.preparationTime * item.quantity;
  });
  UI.totalAmount.textContent = "$" + totalPrice.toFixed(2);
  UI.totalPreparation.textContent = totalPreparation.toFixed(1) + " m";
}

async function render() {
  items = await getCartItems();
  UI.basketBody.innerHTML = "";
  UI.emptyMs.textContent = "Loading basket...";

  if (!items.length) {
    UI.emptyMs.textContent = "Nothing has been added";
    UI.totalAmount.textContent = "$0.00";
    return;
  }

  UI.emptyMs.textContent = "";
  showItemCart();
}

function showProduct(id) {
  const product = products.data.items.find((p) => p.productId === id);
  if (!product) return;
  UI.dialog.showModal();
  UI.dialogImage.setAttribute("src", SERVER_URL + product.imageUrl);
  UI.dialogTitle.textContent = product.name;
  UI.dialogPrice.textContent = "$" + product.price;
  UI.dialogDescription.textContent =
    "The estimated time to prepare this meal is " +
    product.preparationTime +
    " m, Enjoy! your meal and good health.";
}

(async function boot() {
  try {
    await initDB();
    products = await getAllProducts();
    render();
  } catch (e) {
    console.error(`❌ Failed to initialize app: ${e.message}`);
  }
})();

UI.backShop.addEventListener("click", () => {
  window.location.href = "../../pages/public/menu.html";
});

UI.clearAll.addEventListener("click", () => {
  if (!confirm("Are you sure you need clear all?")) return;

  clearAll();
  render();
});

UI.basketBody.addEventListener("click", (e) => {
  const id = Number(e.target.dataset.id);

  if (e.target.classList.contains("increase")) {
    increase(id);
    render();
  }

  if (e.target.classList.contains("decrease")) {
    decrease(id);
    render();
  }

  if (e.target.classList.contains("btn-danger")) {
    if (!confirm("Are you sure?")) return;
    remove(id);
    render();
  }

  if (e.target.classList.contains("show-more")) {
    showProduct(id);
  }
});

UI.close.addEventListener("click", () => {
  UI.dialog.close();
});
