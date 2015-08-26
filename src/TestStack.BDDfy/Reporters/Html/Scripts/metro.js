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


    $("#filterOptions a").click(function () {
        var tile = $(this).children("div");

        tile.toggleClass("filterTileDisabled");

        var show = !tile.hasClass("filterTileDisabled");

        var resultType = tile.data("target-class");

        $("div." + resultType).closest(".scenario").toggle(show);
    });
});