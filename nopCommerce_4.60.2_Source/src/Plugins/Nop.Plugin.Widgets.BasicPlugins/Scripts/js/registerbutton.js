//function submitForm() {
//    var data = $("#registerForm").serialize();
//    console.log(data);
//    alert(data);
//    $.ajax({
//        type: 'POST',
//        url: '/RegisterCustomer/Register',
//        contentType: 'application/x-www-form-urlencoded; charset=UTF-8', // when we use .serialize() this generates the data in query string format. this needs the default contentType (default content type is: contentType: 'application/x-www-form-urlencoded; charset=UTF-8') so it is optional, you can remove it
//        data: data,
//        success: function (result) {
//            alert('Successfully received Data ');
//            console.log(result);
//        },
//        error: function () {
//            alert('Failed to receive the Data');
//            console.log('Failed ');
//        }
//    })
//}

$(document).ready(function () {
    $("#loginForm").validate({
        rules: {
            email: {
                required: true,
                email: true
            },
            password: {
                required: true,
                minlength: 6
            }
        },
        messages: {
            email: {
                required: "Please enter your email address",
                email: "Please enter a valid email address"
            },
            password: {
                required: "Please enter your password",
                minlength: "Your password must be at least 6 characters long"
            }
        },
        submitHandler: function (form) {
            // code to submit the form
        }
    });
});
