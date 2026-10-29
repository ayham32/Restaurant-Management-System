import { Statistics, RecentOrders } from "../api/dashboard.js";

const countIsActive = document.getElementById("countActive");
const countProducts = document.getElementById("countProducts");
const todayOrdersCount = document.getElementById("todayOrdersCount");
const todayRevenue = document.getElementById("todayRevenue");
const Body = document.getElementById("tBody");

async function loadStatistics() {
  const statistics = await Statistics();
  countIsActive.textContent = `${statistics.activeEmployeesCount} is active`;
  countProducts.textContent = statistics.productsCount;
  todayOrdersCount.textContent = statistics.todayOrdersCount;
  todayRevenue.textContent = statistics.todayRevenue;
}

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

(() => {
  loadStatistics();
  loadRecentOrders();
})();
