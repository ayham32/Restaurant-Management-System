import { getBills, addNew } from "./api/bills.js";
import { createShowMessage } from "./components/show-message.js";
import { createPageHeader } from "./components/page-header.js";

const UI = {
  pageHeader: document.getElementById("pageHeader"),
  mess: document.getElementById("mess"),
  btnAdd: document.getElementById("btnAdd"),
  cardsBox: document.getElementById("cardsBox"),
  addNewBill: document.getElementById("addNewBill"),
  addBillForm: document.getElementById("addBillForm"),
  input: {
    orderId: document.getElementById("orderId"),
    taxAmount: document.getElementById("taxAmount"),
    discount: document.getElementById("discount"),
    paymentMethod: document.getElementById("paymentMethod"),
    paymentStatus: document.getElementById("paymentStatus"),
  },
};

let bills = [];

UI.pageHeader.appendChild(createPageHeader("Bills Management"));

async function allBills() {
  return await getBills();
}

const paymentMethod = new Map([
  [0, "Online"],
  [1, "Card"],
  [2, "Cash"],
]);
const paymentStatus = new Map([
  [0, "Unpaid"],
  [1, "Paid"],
]);

function fillBills() {
  UI.cardsBox.innerHTML = "";
  bills.forEach((bill) => {
    const card = document.createElement("div");
    card.classList.add("card");
    card.innerHTML = `
      <h3>#${bill.billId}</h3>
      <p>Order ID: ${bill.orderId}</p>
      <p>Sub Total: ${bill.subTotal}$</p>
      <p>Tax: ${bill.taxAmount}$</p>
      <p>Discount: ${bill.discount}%</p>
      <p>Final Amount: ${bill.finalAmount}$</p>
      <p>Payment Method: ${paymentMethod.get(bill.paymentMethod)}</p>
      <p>Payment Stauts: ${paymentStatus.get(bill.paymentStatus)}</p>
      <p>Issued At: ${bill.issuedAt}</p>
      <p>Created By: ${bill.createdBy}</p>
      <button class = "btnPrint" data-id="${bill.billId}">Print</button>
    `;

    UI.cardsBox.appendChild(card);
  });
}

function printBill(billId) {
  const printWindow = window.open("", "_blank", "width=800,height=600");
  const bill = bills.find((b) => b.billId === billId);
  if (!bill) return;
  if (!printWindow) {
    alert("Please allow pop-ups to print the invoice.");
    return;
  }

  printWindow.document.write(`
    <!DOCTYPE html>
    <html lang="en">
      <head>
        <meta charset="UTF-8">
        <title>Invoice #${bill.billId}</title>
        <style>
          body {
            font-family: Arial, sans-serif;
            padding: 30px;
            color: #111;
          }

          h2, h3 {
            text-align: center;
          }

          .total {
            margin-top: 20px;
            border-top: 1px solid #333;
            padding-top: 10px;
            font-weight: bold;
          }

          @media print {
            body {
              padding: 0;
            }
          }
        </style>
      </head>
      <body>
        <h2>Restaurant Name</h2>
        <h3>Invoice #${bill.billId}</h3>

        <p>Order ID: ${bill.orderId}</p>
        <p>Issued At: ${bill.issuedAt}</p>
        <p>Subtotal: $${Number(bill.subTotal).toFixed(2)}</p>
        <p>Tax: $${Number(bill.taxAmount).toFixed(2)}</p>
        <p>Discount: $${Number(bill.discount).toFixed(2)}</p>

        <div class="total">
          Final Amount: $${Number(bill.finalAmount).toFixed(2)}
        </div>
      </body>
    </html>
  `);

  printWindow.document.close();
  printWindow.focus();

  printWindow.onload = () => {
    printWindow.print();
  };
}

function render() {
  fillBills();
}

async function boot() {
  try {
    bills = await allBills();

    if (!bills) return;

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

function rest() {
  UI.input.discount.value = "";
  UI.input.orderId.value = "";
  UI.input.taxAmount.value = "";
  UI.input.paymentMethod.value = 0;
  UI.input.paymentStatus.value = 0;
}
function addBill() {
  rest();
  UI.addNewBill.showModal();
}
UI.btnAdd.addEventListener("click", addBill);

async function save() {
  const bill = {
    orderId: Number(UI.input.orderId.value),
    taxAmount: UI.input.taxAmount.value ? Number(UI.input.taxAmount.value) : 0,
    discount: UI.input.discount.value ? Number(UI.input.discount.value) : 0,
    paymentMethod: Number(UI.input.paymentMethod.value),
    paymentStatus: Number(UI.input.paymentStatus.value),
  };
  console.log(bill);

  try {
    await addNew(bill);
    showMessage("Added suceessfully.");
    UI.addNewBill.close();
    boot();
  } catch (error) {
    console.error(error);

    showMessage(error.message, false);
  }
}

UI.addBillForm.addEventListener("submit", (event) => {
  event.preventDefault();
  save();
});

UI.cardsBox.addEventListener("click", async (event) => {
  const Id = Number(event.target.dataset.id);
  if (event.target.classList.contains("btnPrint")) {
    printBill(Id);
  }
});
