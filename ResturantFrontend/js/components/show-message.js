export function createShowMessage(mess, isSuccess = true) {
  const div = document.createElement("div");
  const p = document.createElement("p");
  const span = document.createElement("span");

  div.className = "box-message";
  span.textContent = mess;
  if (isSuccess) {
    p.textContent = "Successfully";
    p.style.color = "springgreen";
  } else {
    p.textContent = "Failed";
    p.style.color = "crimson";
  }
  div.append(p, span);
  return div;
}
