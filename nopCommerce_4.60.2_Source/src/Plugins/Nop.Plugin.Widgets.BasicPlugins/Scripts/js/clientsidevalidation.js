const usernameEl = document.querySelector('#username-input');
const passwordEl = document.querySelector('#password-input');

const form = document.querySelector('#signup');
const isRequired = value => value === '' ? false : true;
const isBetween = (length, min, max) => length < min || length > max ? false : true;

const isPasswordSecure = (password) => {
    const re = new RegExp("^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#\$%\^&\*])(?=.{8,})");
    return re.test(password);
};

const showError = (input, message) => {
    // get the form-field element
    const formField = input.parentElement;
    // add the error class
    formField.classList.remove('success');
    formField.classList.add('error');

    // show the error message
    const error = formField.querySelector('small');
    error.textContent = message;
};

const showSuccess = (input) => {
    // get the form-field element
    const formField = input.parentElement;

    // remove the error class
    formField.classList.remove('error');
    formField.classList.add('success');

    // hide the error message
    const error = formField.querySelector('small');
    error.textContent = '';
}

const checkUsername = () => {

    let valid = false;
    const min = 3,
        max = 25;
    const username = usernameEl.value.trim();

    if (!isRequired(username)) {
        showError(usernameEl, 'Username cannot be blank.');
    } else if (!isBetween(username.length, min, max)) {
        showError(usernameEl, `Username must be between ${min} and ${max} characters.`)
    } else {
        showSuccess(usernameEl);
        valid = true;
    }
    return valid;
}


const checkPassword = () => {

    let valid = false;

    const password = passwordEl.value.trim();

    if (!isRequired(password)) {
        showError(passwordEl, 'Password cannot be blank.');
    }
    //else if (!isPasswordSecure(password)) {
    //    showError(passwordEl, 'Password must has at least 8 characters that include at least 1 lowercase character, 1 uppercase characters, 1 number, and 1 special character in (!@#$%^&*)');
    //}
        else {
        showSuccess(passwordEl);
        valid = true;
    }

    return valid;
};

//form.addEventListener('submit', function (e) {
//    // prevent the form from submitting
//    e.preventDefault();

//    // validate forms
//    let isUsernameValid = checkUsername(),
//        isPasswordValid = checkPassword();

//    let isFormValid = isUsernameValid &&
//        isPasswordValid;

//    // submit to the server if the form is valid
//    if (isFormValid) {
//        return true;
//    }
//});

form.addEventListener('input', function (e) {
    switch (e.target.id) {
        case 'username-input':
            checkUsername();
            break;
        case 'password-input':
            checkPassword();
            break;
    }
});


//$('#username-input').on('change', function () {
//    //alert("The text has been changed.");
//    checkUsername();
//    //showSuccess(usernameEl);
//});

//$('#password-input').on('change', function () {
//    checkPassword();
//});

//function val() {

//    //e.preventDefault();

//    // validate forms
//    let isUsernameValid = checkUsername(),
//        isPasswordValid = checkPassword();

//    let isFormValid = isUsernameValid &&
//        isPasswordValid;

//    // submit to the server if the form is valid
//    if (isFormValid) {
//        return true;
//    }
//}
