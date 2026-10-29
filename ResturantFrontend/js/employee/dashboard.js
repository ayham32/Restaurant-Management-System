import { Statistics, RecentOrders } from "../api/dashboardEmployee.js";

const Body = document.getElementById("tBody");
const cancelledOrderCount = document.getElementById("cancelledOrderCount");
const completedOrderCount = document.getElementById("completedOrderCount");
const cookingOrderCount = document.getElementById("cookingOrderCount");
const pendingOrderCount = document.getElementById("pendingOrderCount");
const isntAvilableProductsCount = document.getElementById(
  "isntAvilableProductsCount",
);
const productName = document.getElementById("productName");
const todaysSalesPrices = document.getElementById("todaysSalesPrices");

async function loadRecentOrders() {
  const recentOrders = await RecentOrders();

  recentOrders.forEach((element) => {
    const tr = document.createElement("tr");
    tr.innerHTML = `
    <td>${element.id}</td>
                 <td>${element.customerName ?? "Guest"}</td>
                 <td>${element.totalPrice}</td>
                  <td>${element.status}</td>
                  `;
    Body.appendChild(tr);
  });
}

async function loadStatistics() {
  const statistics = await Statistics();
  cancelledOrderCount.textContent = statistics.cancelledOrderCount;
  completedOrderCount.textContent = statistics.completedOrderCount;
  cookingOrderCount.textContent = statistics.cookingOrderCount;
  pendingOrderCount.textContent = statistics.pendingOrderCount;
  isntAvilableProductsCount.textContent = statistics.isntAvilableProductsCount;
  todaysSalesPrices.textContent = statistics.todaysSalesPrices;

  if (!statistics.isntAvilableProductsCount) {
    const li = document.createElement("li");
    li.textContent = "No product is not avilable";
    productName.appendChild(li);
  }
  statistics.productNameIsntAvilable.forEach((p) => {
    const li = document.createElement("li");
    li.textContent = p;
    productName.appendChild(li);
  });
}

(() => {
  loadStatistics();
  loadRecentOrders();
})();
