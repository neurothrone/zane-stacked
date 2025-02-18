function setTheme(theme) {
    document.documentElement.setAttribute("data-bs-theme", theme);
}

window.getWindowWidth = () => {
    return window.innerWidth;
};

document.addEventListener("DOMContentLoaded", function () {
    let savedTheme = localStorage.getItem("theme") || "dark";
    setTheme(savedTheme);
});