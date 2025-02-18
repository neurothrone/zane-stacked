function setTheme(theme) {
    document.documentElement.setAttribute("data-bs-theme", theme);
}

document.addEventListener("DOMContentLoaded", function () {
    let savedTheme = localStorage.getItem("theme") || "dark";
    setTheme(savedTheme);
});