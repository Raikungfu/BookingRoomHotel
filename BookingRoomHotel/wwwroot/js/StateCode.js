document.addEventListener("DOMContentLoaded", () => {
    if (checkRole()) {
        document.getElementById("showName").innerText = "Welcome, " + localStorage.getItem("Name");
    }
    if (checkRole()) {
        document.getElementById("showLogout").classList.remove("invisible");
        if (roleCus()) {
            document.getElementById("showLoginStaff").classList.remove("invisible");
        } else {
            document.getElementById("showLogResCus").classList.remove("invisible");
        }
    } else {
        document.getElementById("showLoginStaff").classList.remove("invisible");
        document.getElementById("showLogResCus").classList.remove("invisible");
    }
});

function checkRole() {
    return localStorage.getItem("Role") !== null;
}
function roleCus() {
    return localStorage.getItem("Role") === "customer";
}

function logout() {
    localStorage.clear();
    document.getElementById("showLogout").classList.add("invisible");
    document.getElementById("showLoginStaff").classList.remove("invisible");
    document.getElementById("showLogResCus").classList.remove("invisible");
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
    alert(endPoint);
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
    return false;
}