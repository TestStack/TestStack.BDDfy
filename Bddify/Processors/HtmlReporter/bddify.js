$(function () {
    $('.canToggle').each(function () {
        var target = $('#' + $(this).data('toggle-target'));
        target.hide();
        $(this).click(function () {
            target.toggle(200);
        });
    });

    $('.expandAll').click(function () {
        $('.steps').css('display', '');
    });
    $('.collapseAll').click(function () {
        $('.steps').css('display', 'none');
    });
});