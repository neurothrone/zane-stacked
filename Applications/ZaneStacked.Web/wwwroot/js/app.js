function setTheme(theme) {
    document.documentElement.setAttribute("data-bs-theme", theme);
}

window.getWindowWidth = () => {
    return window.innerWidth;
};

window.focusElement = (element) => {
    if (element) {
        element.focus();
    }
};

window.setAdminTheme = function () {
    document.documentElement.setAttribute("data-bs-theme", "dark");
};

document.addEventListener("DOMContentLoaded", function () {
    let savedTheme = localStorage.getItem("theme") || "dark";
    setTheme(savedTheme);
});