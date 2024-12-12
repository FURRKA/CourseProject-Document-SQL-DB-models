namespace BLL.Interfaces
{
    public interface IStatisticService
    {
        public void ShowPaymentStatistic(DateTime date1, DateTime date2);
        public void ShowRouteStatistic(DateTime date1, DateTime date2);
    }
}
