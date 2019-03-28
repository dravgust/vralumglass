// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

$('.carousel').carousel({
    interval: 8000,
    wrap: true,
    pouse: "hover",
    touch: true
});

$('#submit').on('click', function (evt) {
    if (!$('form').valid()) {
        $('#loadingModal').modal('hide');
    } else {
        $('#loadingModal').modal('show');
    }
});

$(".custom-file-input").on("change", function () {
    var fileName = $(this).val().split("\\").pop();
    $(this).siblings(".custom-file-label").addClass("selected").html(fileName);
});
