// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
// Write your JavaScript code.]
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
            window.location.href = '/Home/Index';
        } else {
            throw new Error(data.error);
        }
    }).catch(error => {
        var errorStatus = document.createElement('span');
        errorStatus.className = 'text-danger';
        errorStatus.textContent = error;
        form.appendChild(errorStatus);
        window.setTimeout(function () {
            errorStatus.remove();
        }, 5000);
    });
}