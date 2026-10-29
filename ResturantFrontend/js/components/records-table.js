export function createRecordsTable(records) {
  const p = document.createElement("p");
  p.textContent = `Records = ${records}`;
  return p;
}
