$(document).ready(function () {
    $(document).on('mousedown', '.show-pass', function () {
        $(this).prev().attr('type', 'text');
        $(this).children('i').removeClass('fa-eye-slash').addClass('fa-eye');
    })
    $(document).on('mouseup', '.show-pass', function () {
        $(this).prev().attr('type', 'password');
        $(this).children('i').removeClass('fa-eye').addClass('fa-eye-slash')
    })
    
    $('.menu-toggle').on('click', function () {
        $('nav > ul').slideToggle();
    })
})
