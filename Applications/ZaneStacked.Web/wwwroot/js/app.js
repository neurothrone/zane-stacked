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

window.showBootstrapToast = (title, message, colorClass) => {
    let toastEl = document.getElementById("blazorToast");
    if (toastEl) {
        document.getElementById("toastTitle").textContent = title;
        document.getElementById("toastMessage").textContent = message;

        let icon = document.getElementById("toastIcon");
        icon.className = `bi bi-exclamation-circle-fill ${colorClass}`;

        let toast = new bootstrap.Toast(toastEl);
        toast.show();
    }
};

document.addEventListener("DOMContentLoaded", function () {
    let savedTheme = localStorage.getItem("theme") || "dark";
    setTheme(savedTheme);
});