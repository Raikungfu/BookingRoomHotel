// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.]

document.addEventListener("DOMContentLoaded", () => {
    if (checkRole()) {
        document.getElementById("showName").innerText = "Welcome, " + localStorage.getItem("Name");
        document.getElementById("showLogout").classList.remove("d-none");
    }else {
        document.getElementById("showLoginStaff").classList.remove("d-none");
        document.getElementById("showLogResCus").classList.remove("d-none");
    }
    
});

function checkRole() {
    return localStorage.getItem("Role") !== null;
}

function loginCustomer() {
    var formLogin = document.getElementById('loginForm');
    sendToCustomerController('/Customers/Login', formLogin);
    return false;
}

function registerCustomer() {
    var formRegister = document.getElementById('registerForm');
    sendToCustomerController('/Customers/Register', formRegister);
    return false;
}

function changePasswordCustomer() {
    var formChangePassword = document.getElementById('changePasswordForm');
    sendToCustomerController('/Customers/ChangePassword', formChangePassword);
    return false;
}

function forgotPasswordCustomer() {
    var formForgotPassword = document.getElementById('forgotPasswordForm');
    sendToCustomerController('/Customers/ForgotPassword', formForgotPassword);
    return false;
}

function sendToCustomerController(endPoint, form) {
    form.addEventListener('submit', (e) => e.preventDefault());
    const dataToSend = new FormData(form);
    fetch(endPoint, {
        method: "POST",
        body: dataToSend
    }).then(response => {
        if (response.ok) return response.json();
        else throw new Error('Network was not ok! Status: ' + response.status);
    }).then(data => {
        if (data.success === true) {
            if (data.accessToken !== null) localStorage.setItem("accessToken", data.accessToken);
            if (data.role !== null) localStorage.setItem("Role", data.role);
            if (data.name !== null) localStorage.setItem("Name", data.name);
            hideLogRegBtn();
            document.getElementById('successMessage').innerHTML = data.message;
            $('.modal').modal().hide();
            $('.modal').on('hidden.bs.modal', function () {
                $('.modal-backdrop').remove();
            });
            $('#successModal').modal('show');
        }
        else {
            throw new Error(data.error);
        }
    }).catch(error => {
        document.getElementById('errorMessage').innerHTML = error;
        $('#errorModal').modal('show');
    });
}

function logout() {
    localStorage.clear();
    document.getElementById("showLogout").classList.add("d-none");
    document.getElementById("showLoginStaff").classList.remove("d-none");
    document.getElementById("showLogResCus").classList.remove("d-none");
}

var jwtToken = localStorage.getItem("accessToken");
function sendJwt(endPoint, id) {
    if (id !== '') {
        endPoint += id;
    }
    fetch(endPoint, {
        method: "GET",
        headers: {
            "Authorization": `Bearer ${jwtToken}`
        }
    }).then(response => {
        if (response.ok) {
            return response.text();
        } else {
        }
    }).then(data => {
        document.getElementById("listData").innerHTML = data;
    }).catch(error => {
        alert(error);
    });
    return false;
}

function sendJwtAndData(endPoint, id) {
    var form = document.getElementById("formData");
    const dataToSend = new FormData(form);
    if (id !== '') {
        endPoint += id;
    }
    fetch(endPoint, {
        method: "POST",
        headers: {
            "Authorization": `Bearer ${jwtToken}`
        },
        body: dataToSend
    }).then(response => {
        if (response.ok) {
            return response.text();
        } else {
        }
    }).then(data => {
        document.getElementById("listData").innerHTML = data;
    }).catch(error => {
        alert(error);
    });
}

function hideLogRegBtn() {
    document.getElementById("showName").innerText = "Welcome, " + localStorage.getItem("Name");
    document.getElementById("showLogout").classList.remove("d-none");
    document.getElementById("showLoginStaff").classList.add("d-none");
    document.getElementById("showLogResCus").classList.add("d-none");
}
