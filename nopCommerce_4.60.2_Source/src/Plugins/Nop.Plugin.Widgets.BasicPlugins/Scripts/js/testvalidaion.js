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


//$(document).ready(function () {
//    $("#loginForm").validate({
//        rules: {
//            email: {
//                required: true,
//                email: true
//            },
//            password: {
//                required: true,
//                minlength: 6
//            }
//        },
//        messages: {
//            email: {
//                required: "Please enter your email address",
//                email: "Please enter a valid email address"
//            },
//            password: {
//                required: "Please enter your password",
//                minlength: "Your password must be at least 6 characters long"
//            }
//        },
//        submitHandler: function (form) {
//            // code to submit the form
//        }
//    });
//});



