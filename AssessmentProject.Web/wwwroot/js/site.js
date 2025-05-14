function eyeToggle(inputId, imageId) {
    var data = document.getElementById(inputId);
    if (data.type === "password") {
        data.type = "text";
        document.getElementById(imageId).src = "../images/eye-slash-fill.svg";
    } else {
        data.type = "password";
        document.getElementById(imageId).src = "../images/eye-fill.svg";
    }
}

toastr.options = {
    "closeButton": true,
    "progressBar": true,
    "positionClass": "toast-top-right",
    "timeOut": "2000"
};
