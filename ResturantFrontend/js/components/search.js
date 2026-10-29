export function createSearch(options, onSearchChange) {
  const div = document.createElement("div");
  const label = document.createElement("label");
  const select = document.createElement("select");
  const input = document.createElement("input");

  div.className = "search-add";
  label.htmlFor = "search";
  label.textContent = "Filter By: ";

  options.forEach((option) => {
    const opt = document.createElement("option");

    opt.value = option.value;
    opt.textContent = option.label;

    select.appendChild(opt);
  });

  input.type = "text";
  input.placeholder = `Search by ${options[0].label}...`;

  select.addEventListener("change", () => {
    input.value = "";

    const selected = options.find((option) => option.value === select.value);

    input.placeholder = `Search by ${selected.label}...`;

    onSearchChange({
      searchBy: select.value,
      searchValue: input.value,
    });
  });

  input.addEventListener("input", () => {
    onSearchChange({
      searchBy: select.value,
      searchValue: input.value,
    });
  });

  div.append(label, select, input);

  return div;
}
