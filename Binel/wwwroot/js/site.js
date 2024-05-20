// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function isValidEmail(email) {
    // Email için geçerli bir düzenli ifade
    var emailRegex = /^[^\s@]+@@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
}

document.addEventListener('DOMContentLoaded', function () {





});


//Bu kodlar, fare arama butonunun üzerine geldiğinde ve üzerinden çekildiğinde resminin değişmesini sağlar. search-bar-01.png ve search-bar-02.png dosyalarının gerçek yolunu belirterek bu kısmı güncelleyebilirsiniz.
function changeImage() {
    document.getElementById("search-img").src = "search-bar-02.png";
}

function restoreImage() {
    document.getElementById("search-img").src = "search-bar-01.png";
}
