namespace AppTienda.Core.Application.Dtos.Finance
{
    public enum DateFilterType
    {
        Hoy = 1,
        EstaSemana = 2,
        EsteMes = 3,
        EsteAno = 4,
        Historico = 5,
        Personalizado = 6
    }

    public class FinanceFilterDto
    {
        public DateFilterType FilterType { get; set; } = DateFilterType.EsteMes;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
