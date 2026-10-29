export function createPagination({
  pageNumber,
  totalCount,
  pageSize,
  onPageChange,
}) {
  const div = document.createElement("div");

  div.className = "pagination";

  const prev = document.createElement("button");
  prev.textContent = "←";

  const page = document.createElement("span");
  page.textContent = pageNumber;

  const next = document.createElement("button");
  next.textContent = "→";

  div.append(prev, page, next);

  const totalPages = Math.ceil(totalCount / pageSize);

  prev.disabled = pageNumber === 1;
  next.disabled = pageNumber >= totalPages;

  prev.addEventListener("click", () => {
    onPageChange(pageNumber - 1);
  });

  next.addEventListener("click", () => {
    onPageChange(pageNumber + 1);
  });

  return div;
}
