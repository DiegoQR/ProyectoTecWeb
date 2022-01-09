const baseRawUrl = "http://localhost:5000";
const baseUrl = `${baseRawUrl}/api`;

window.addEventListener("DOMContentLoaded", function(event){
    window.addEventListener('load', function(event){
        const loginForm = document.getElementById('login-user-form');
        loginForm.addEventListener('submit', loginUser);
    });
});

async function loginUser(event){
    event.preventDefault();
    const loginUrl = `${baseUrl}/auth/Login`;

    if(!Boolean(event.currentTarget["e-mail"].value)){
        var eMailInput = document.getElementById('e-mail');
        eMailInput.placeholder = "The E-Mail is required";
        eMailInput.classList.add('bad-input');
        return 0;
    }

    if(!Boolean(event.currentTarget["password"].value)){
        var eMailInput = document.getElementById('pwd');
        eMailInput.placeholder = "The password is required";
        eMailInput.classList.add('bad-input');
        return 0;
    }

    var data = {
        email: event.currentTarget["e-mail"].value,
        password: event.currentTarget["password"].value
    }

    const params = {
        headers: { "Content-Type": "application/json; charset=utf-8" },
        method: 'POST',
        body: JSON.stringify(data)
    }

    var response = await fetch(loginUrl, params);
    if(response.status == 200){
        var data = await response.json();
        if(data.isSuccess){
            sessionStorage.setItem("jwt", data.token);
            window.location.href = "home.html";
        } else {
            var errors = data.errors.join("\n") ?? ""
            alert(data.token + errors);
            location.reload();
        }
    } else {
        var data = await response.text()
        alert(data);
    }
}