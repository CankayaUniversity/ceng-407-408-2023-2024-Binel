document.addEventListener('DOMContentLoaded', function () {
    $('.filter-button_0').click(function () {
        filterPosts(0);
    });
    $('.filter-button_1').click(function () {
        filterPosts(1);
    });
    $('.filter-button_2').click(function () {
        filterPosts(2);
    });
    $('.filter-button_3').click(function () {
        filterPosts(3);
    });
    $('.filter-button_4').click(function () {
        filterPosts(4);
    });

    function filterPosts(filterId) {
        $.ajax({
            url: '/feed/filter',
            type: 'GET',
            data: { filterId: filterId },
            success: function (data) {
                // Gelen veriyi konsola yazdır
                console.log("Gelen Veri:", data);

                // Sayfanın içeriğini güncelle
                $('#content-container').html(data);
            },
            error: function (error) {
                console.error('Filtreleme işlemi sırasında hata oluştu:', error);
            }
        });
    }
});




// Bu kodlar, fare arama butonunun üzerine geldiğinde ve üzerinden çekildiğinde resminin değişmesini sağlar.
function changeImage() {
    document.getElementById("search-img").src = "search-bar-02.png";
}

function restoreImage() {
    document.getElementById("search-img").src = "search-bar-01.png";
}
