//Asigna el control daterangepicker a todos los controles que tengan en su id "txtFecha"
$(function () {
    $('input[id*="txtFiFecha"]').daterangepicker({
        "locale": {
            cancelLabel: 'Clear',
            "format": "DD/MM/YYYY",
            "separator": " - ",
            "applyLabel": "APLICAR",
            "cancelLabel": "CANCELAR",
            "fromLabel": "De",
            "toLabel": "A",
            "customRangeLabel": "Personalizar...",
            "weekLabel": "W",
            "daysOfWeek": [
                "Do",
                "Lu",
                "Ma",
                "Mi",
                "Ju",
                "Vi",
                "Sa"
            ],
            "monthNames": [
                "Enero",
                "Febrero",
                "Marzo",
                "Abril",
                "Mayo",
                "Junio",
                "Julio",
                "Agosto",
                "Septiembre",
                "Octubre",
                "Noviembre",
                "Diciembre"
            ],
            "firstDay": 1
        },
        opens: 'center',
        autoUpdateInput: false
    }, function (start, end, label) {
        console.log("A new date selection was made: " + start.format('DD-MM-YYYY') + ' to ' + end.format('DD-MM-YYYY'));
    });

    $('input[id*="txtFiFecha"]').on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('DD/MM/YYYY') + ' - ' + picker.endDate.format('DD/MM/YYYY'));
        $('input[id*="txtFiFecha"]').addClass("edited");
    });

    $('input[id*="txtFiFecha"]').on('cancel.daterangepicker', function (ev, picker) {
        $(this).val('');
        $('input[id*="txtFiFecha"]').removeClass("form-control edited").addClass("form-control");
    });
});