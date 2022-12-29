$(document).ready(function () {
    $(document).on('mousedown', '.show-pass', function () {
        $(this).prev().attr('type', 'text');
        $(this).children('i').removeClass('fa-eye-slash').addClass('fa-eye');
    })
    $(document).on('mouseup', '.show-pass', function () {
        $(this).prev().attr('type', 'password');
        $(this).children('i').removeClass('fa-eye').addClass('fa-eye-slash')
    })
    $('.show-hide-btn').on('click', function (e) {
        e.preventDefault();
        $(this).next('ul').slideToggle();
    })
    
     $('.menu-toggle').on('click', function () {
        $('nav > ul').slideToggle();
    })
    
    $(document).bind("DOMSubtreeModified",function(){
        $('[data-toggle="tooltip"]').tooltip();
    })
    
    
    $(document).on('click', '.message-submit', function (e) {
       e.preventDefault();
       $('.submit-message').show();
    })
    
    $(document).on('click', '.btn-yes', function (e) {
        e.preventDefault();
        $('.submit-message').hide();
        $('.submit-btn button').trigger('click');
    })
    
    $(document).on('click', '.btn-no', function (e) {
        e.preventDefault();
        $('.message-submit').show();
        $('.submit-message').hide();
    })
    
})


