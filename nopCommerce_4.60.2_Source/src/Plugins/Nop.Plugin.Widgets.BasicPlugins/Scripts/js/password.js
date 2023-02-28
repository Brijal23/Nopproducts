function password_check() {
    pass = document.getElementById("password").value;
    console.log(pass);
    regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
    if (regex.exec(pass) == null) {
        alert('invalid password!')
    }
    else {
        console.log("valid");
    }
}



