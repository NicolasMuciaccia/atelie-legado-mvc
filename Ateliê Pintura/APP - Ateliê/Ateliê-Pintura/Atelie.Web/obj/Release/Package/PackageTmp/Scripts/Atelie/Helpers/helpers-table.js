Atelie.Helpers.TableFormat = {};

Atelie.Helpers.TableFormat.aplicarLarguraHeader = function(selectorLinhaTH, larguras) {
    const $ths = $(selectorLinhaTH).find('th');

    $ths.each(function (index) {
        if (larguras[index]) {
            $(this).css('width', larguras[index]);
        }
    });
}