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

window.enableNavRailTooltips = () => {
    const navRail = document.querySelector('.navrail');
    if (!navRail)
        return;

    const tooltipTriggerList = Array.from(navRail.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.forEach(tooltipTriggerEl => {
        new bootstrap.Tooltip(tooltipTriggerEl);
    });
};


document.addEventListener("DOMContentLoaded", function () {
    let savedTheme = localStorage.getItem("theme") || "dark";
    setTheme(savedTheme);
});