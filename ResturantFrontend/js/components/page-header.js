export function createPageHeader(title, image, alt = "") {
  const div = document.createElement("div");

  div.className = "head-main";

  div.innerHTML = `
        <h2>${title}</h2>
        ${image ? `<img src="${image}" alt="${alt}">` : ""}
    `;

  return div;
}
