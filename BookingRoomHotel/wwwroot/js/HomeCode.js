﻿document.addEventListener('DOMContentLoaded', () => {
    if (window.location.pathname === "/Admin" || window.location.pathname === "/Admin/Dashboard") {
        document.getElementById("navHome").classList.add("d-none");
        document.getElementById("footer").classList.add("d-none");
    }
});