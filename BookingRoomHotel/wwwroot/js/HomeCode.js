document.addEventListener('DOMContentLoaded', () => {
    alert(window.location.pathname);
    if (window.location.pathname === "Admin" || (window.location.pathname === "Dashboard") {
        document.getElementById("navHome").classList.add("d-none");
        document.getElementById("footer").classList.add("d-none");
    }
});