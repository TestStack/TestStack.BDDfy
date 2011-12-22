$(function () {
    $('.expandAll').click(function () {
        $('.steps').css('display', '');
    });
    $('.collapseAll').click(function () {
        $('.steps').css('display', 'none');
    });
});

function toggle(id) {
    var e = document.getElementById(id);
    if (e.style.display == 'none') {
        e.style.display = '';
    }
    else {
        e.style.display = 'none';
    }
}
