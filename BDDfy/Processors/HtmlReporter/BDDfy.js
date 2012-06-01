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

    $("ul.resultSummary li:not('.storySummary'):not('.scenarioSummary')")
        .append("<input type='checkbox' class='cbx_toggle' checked/>");

    $(".cbx_toggle").click(function () {

        var checkBox = $(this);
        var resultType = checkBox.closest("li").attr("class");

        $("#testResult div.scenario ." + resultType).parent().toggle(checkBox.is(":checked"));
    });
});