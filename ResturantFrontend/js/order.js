import {
  getOrders,
  addNewOrder,
  updateOrder,
  deleteOrder,
  addItem,
  deleteItem,
  updateItem,
  cancelledOrder,
  completedOrder,
  cookingOrder,
  pendingOrder,
} from "./api/orders.js";
import { createShowMessage } from "./components/show-message.js";
import { createPageHeader } from "./components/page-header.js";

const UI = {
  pageHeader: document.getElementById("pageHeader"),
  mess: document.getElementById("mess"),
  cardsBox: document.getElementById("cardsBox"),
  btnAdd: document.getElementById("btnAdd"),
  addEditOrder: document.getElementById("addEditOrder"),
  title: document.getElementById("title"),
  addEditOrderForm: document.getElementById("addEditOrderForm"),
  input: {
    customerId: document.getElementById("customerId"),
    orderType: document.getElementById("orderType"),
    tableNumber: document.getElementById("tableNumber"),
  },
  addItem: document.getElementById("addItem"),
  addItemForm: document.getElementById("addItemForm"),
  addItemInput: {
    products: document.getElementById("products"),
    btnShowProducts: document.getElementById("btnShowProducts"),
    quantity: document.getElementById("quantity"),
  },
  editItem: document.getElementById("editItem"),
  editItemForm: document.getElementById("editItemForm"),
  editItemInput: {
    eQuantity: document.getElementById("eQuantity"),
  },
};

let orders = [];
let selectedOrderId = -1;
let mode = "Add";
let selectedOrderItemId = -1;

UI.pageHeader.appendChild(
  createPageHeader(
    "Orders Management",
    "../../assets/images/purchase-order.png",
    "Orders Image",
  ),
);

async function allOrders() {
  return await getOrders();
}

const orderType = new Map([
  [0, "Takeaway"],
  [1, "Delivery"],
  [2, "DineIn"],
]);
const orderStatus = new Map([
  [0, "Cancelled"],
  [1, "Completed"],
  [2, "Cooking"],
  [3, "Pending"],
]);

function fillOrders() {
  UI.cardsBox.innerHTML = "";
  orders.forEach((order) => {
    const card = document.createElement("div");
    card.classList.add("card");
    card.innerHTML = `
        <h3>#${order.orderId}</h3>
        <p> ${order.customerName || "Guest"}</p>
        <p> Order Type: ${orderType.get(order.orderType)}</p>
        <p> Order Status: ${orderStatus.get(order.status)}</p> 
        <p> Created By: ${order.createdByName ?? "Admin"}</p>
        <p> Created: ${order.orderDate}</p>
        <p> Table Number: ${order.tableNumber || "No table"}</p>
       <div>
        <h3>Items:</h3>
        <button class="btnAddItem" data-order-id="${order.orderId}">Add item</button>
        </div> 
        <div class="order-items"></div>
        <p> Total Amount: ${order.totalAmount}</p>
        <button class="btnEdit" data-order-id="${order.orderId}">Edit</button>
      <button class="btnDelete"  data-order-id="${order.orderId}">Delete</button>
      <div> 
         <button class="btnCancelled" data-order-id="${order.orderId}">Cancelled</button>
        <button class="btnCompleted" data-order-id="${order.orderId}">Completed</button>
        <button class="btnCooking" data-order-id="${order.orderId}">Cooking</button>
        <button class="btnPending" data-order-id="${order.orderId}">Pending</button> 
      </div>
      `;
    const itemsBox = card.querySelector(".order-items");

    if (!order.items.length) {
      const item = document.createElement("div");
      item.className = "order-item";

      item.textContent = "No items for this order";
      itemsBox.appendChild(item);
    }

    order.items.forEach((oi) => {
      const item = document.createElement("div");
      item.className = "order-item";
      item.innerHTML = `
      <h3>#${oi.orderItemId}</h3>
      <p>Product: ${oi.productName}
       </p>
      <p>Quantity: ${oi.quantity}</p>
      <p>Unit Price: ${oi.unitPrice}</p>
      <button class="btnEditItem" 
      data-order-id="${order.orderId}"
      data-item-id="${oi.orderItemId}">
      Edit</button>
      <button class="btnDeleteItem"
      data-order-id="${order.orderId}"
      data-item-id="${oi.orderItemId}">
      Delete</button>
    `;
      itemsBox.appendChild(item);
    });

    UI.cardsBox.appendChild(card);
  });
}

function render() {
  fillOrders();
}

async function boot() {
  try {
    orders = await allOrders();
    console.log(orders);

    if (!orders) return;

    render();
  } catch (error) {
    console.error(error);
    showMessage(error, false);
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

function rest() {
  UI.input.customerId.value = "";
  UI.input.orderType.value = 1;
  UI.input.tableNumber.value = "";
}
function addOrder() {
  rest();
  mode = "Add";
  UI.addEditOrder.showModal();
  UI.title.textContent = "Add New Order";
}

function editOrder(Id) {
  const order = orders.find((o) => o.orderId === Id);
  if (!order) return;
  mode = "Edit";
  UI.addEditOrder.showModal();
  UI.title.textContent = "Edit Order";
  UI.input.customerId.value = order.customerId;
  UI.input.orderType.value = order.orderType;
  UI.input.tableNumber.value = order.tableNumber;
}

async function Save() {
  const order = {
    customerId: UI.input.customerId.value
      ? Number(UI.input.customerId.value)
      : null,
    orderType: UI.input.orderType.value
      ? Number(UI.input.orderType.value)
      : null,
    tableNumber: Number(UI.input.orderType.value),
  };
  console.log(order);

  try {
    if (mode === "Add") {
      await addNewOrder(order);
      showMessage("Added order successfully");
    } else {
      await updateOrder(selectedOrderId, order);
      showMessage("Updated order successfully");
    }
    UI.addEditOrder.close();

    boot();
  } catch (error) {
    showMessage(error.message, false);
    console.error(error);
  }
}

UI.btnAdd.addEventListener("click", addOrder);

UI.cardsBox.addEventListener("click", async (event) => {
  const orderId = Number(event.target.dataset.orderId);
  const itemId = Number(event.target.dataset.itemId);
  if (event.target.classList.contains("btnDelete")) {
    if (confirm("Are you sure? when you deleted you cannot retern it!")) {
      await deleteOrder(orderId);
      showMessage("Deleted done!");
      boot();
    }
  }
  if (event.target.classList.contains("btnDeleteItem")) {
    if (confirm("Are you sure? when you deleted you cannot retern it!")) {
      await deleteItem(orderId, itemId);
      showMessage("Deleted done!");
      boot();
    }
  }

  if (event.target.classList.contains("btnEdit")) {
    selectedOrderId = orderId;
    editOrder(orderId);
  }
  if (event.target.classList.contains("btnAddItem")) {
    restAddItem();
    selectedOrderId = orderId;
  }
  if (event.target.classList.contains("btnEditItem")) {
    selectedOrderId = orderId;
    selectedOrderItemId = itemId;
    editItem();
  }
  try {
    if (event.target.classList.contains("btnCancelled")) {
      if (
        confirm(
          "Are you sure? when you cancelled this order you cannot modify it!",
        )
      ) {
        await cancelledOrder(orderId);
        showMessage("Cancelled done!");
        boot();
      }
    }
    if (event.target.classList.contains("btnCompleted")) {
      if (
        confirm(
          "Are you sure? when you completed this order you cannot modify it!",
        )
      ) {
        await completedOrder(orderId);
        showMessage("Completed done!");
        boot();
      }
    }
    if (event.target.classList.contains("btnCooking")) {
      await cookingOrder(orderId);
      showMessage("Cooking done!");
      boot();
    }
    if (event.target.classList.contains("btnPending")) {
      await pendingOrder(orderId);
      showMessage("Pending done!");
      boot();
    }
  } catch (error) {
    showMessage(error.message, false);
  }
});

UI.addEditOrderForm.addEventListener("submit", (e) => {
  e.preventDefault();
  Save();
});

async function addNewItem(orderId) {
  const item = {
    productId: Number(UI.addItemInput.products.value),
    quantity: Number(UI.addItemInput.quantity.value),
  };
  try {
    await addItem(orderId, item);
    showMessage("Added successfully");
    UI.addItem.close();
    boot();
  } catch (error) {
    showMessage(error.message, false);
    console.error(error);
  }
}

function restAddItem() {
  UI.addItem.showModal();
  UI.addItemInput.products.value = "";
  UI.addItemInput.quantity.value = "";
}

UI.addItemForm.addEventListener("submit", (e) => {
  e.preventDefault();
  addNewItem(selectedOrderId);
});

UI.addItemInput.btnShowProducts.addEventListener("click", () => {
  window.location.href = "../../pages/products/product.html";
});

function editItem() {
  const order = orders.find((o) => o.orderId === selectedOrderId);
  const item = order.items.find((oi) => oi.orderItemId === selectedOrderItemId);
  if (!order || !item) return;
  UI.editItem.showModal();
  UI.editItemInput.eQuantity.value = item.quantity;
}

async function saveEditItem() {
  const item = { quantity: Number(UI.editItemInput.eQuantity.value) };
  try {
    await updateItem(selectedOrderId, selectedOrderItemId, item);
    showMessage("Updated successfully.");
    UI.editItem.close();
    boot();
  } catch (error) {
    console.error(error);
    showMessage(error.message, false);
  }
}
UI.editItemForm.addEventListener("submit", (event) => {
  event.preventDefault();
  saveEditItem();
});
