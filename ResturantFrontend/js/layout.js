async function loadComponent(selector, url) {
  const container = document.querySelector(selector);
  if (!container) {
    return;
  }
  const response = await fetch(url);

  container.innerHTML = await response.text();
}

export async function initApp() {
  await loadComponent("#footer", "../../components/footer.html");
  await loadComponent("#header", "../../components/header.html");
  await loadComponent("#switchMode", "../../components/switchMode.html");
  await loadComponent("#sideBar", "../../components/sidebar.html");
}
initApp();
