var Atelie = Atelie || {};
Atelie.Controller = Atelie.Controller || {};
Atelie.Controller.Home = Atelie.Controller.Home || {};

$(function () {
    Atelie.Controller.Home._chartInstance = null;

    Atelie.Controller.Home.CarregarGrafico();

    $('#filtroAno, #filtroPendente').on('change', function () {
        Atelie.Controller.Home.CarregarGrafico();
    });
});

Atelie.Controller.Home.Exportar = function () {
    var filtro = Atelie.Controller.Home.ObterFiltroPagamento();

    var queryString = $.param(filtro);
    var urlFinal = this.UrlExportarRelatorio + '?' + queryString;

    window.location.href = urlFinal;
}

Atelie.Controller.Home.CarregarGrafico = function () {
    var filtro = Atelie.Controller.Home.ObterFiltroPagamento();

    $.get(this.UrlObterDadosGrafico, filtro, function (response) {
        if (!response.Success) {
            alert(response.Message);
            return;
        }

        var dados = response.Object;

        if (Atelie.Controller.Home._chartInstance) {
            Atelie.Controller.Home._chartInstance.destroy();
        }

        var ctx = document.getElementById('graficoPagamentos').getContext('2d');

        Atelie.Controller.Home._chartInstance = new Chart(ctx, {
            type: 'bar',
            data: {
                labels: dados.Labels,
                datasets: [
                    {
                        label: 'Pagos',
                        data: dados.ValoresPagos,
                        backgroundColor: 'rgba(40, 167, 69, 0.6)'
                    },
                    {
                        label: 'Pendentes',
                        data: dados.ValoresPendentes,
                        backgroundColor: 'rgba(253, 126, 20, 0.6)'
                    }
                ]
            },
            options: {
                scales: {
                    y: {
                        beginAtZero: true,
                        ticks: {
                            callback: function (value) {
                                return 'R$ ' + value.toLocaleString('pt-BR', { minimumFractionDigits: 2 });
                            }
                        }
                    }
                },
                plugins: {
                    tooltip: {
                        callbacks: {
                            label: function (context) {
                                let valor = context.parsed.y;
                                return `${context.dataset.label}: R$ ${valor.toLocaleString('pt-BR', { minimumFractionDigits: 2 })}`;
                            }
                        }
                    }
                }
            }
        });
    });
};

Atelie.Controller.Home.ObterFiltroPagamento = function () {
    return {
        AnoFiltro: $('#filtroAno').val() ? parseInt($('#filtroAno').val()) : null,
        MesFiltro: $('#filtroMes').val() == "0" ? null : parseInt($('#filtroMes').val()),
        PendenteFiltro: $('#filtroPendente').val() === "" ? null : ($('#filtroPendente').val() === "true")
    };
};

Atelie.Controller.Home.LimparFiltro = function () {
    $('#filtroFormulario')[0].reset();
    Atelie.Controller.Home.CarregarGrafico();
};
