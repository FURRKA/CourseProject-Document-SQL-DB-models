using BLL.Interfaces;

namespace BLL.DTO
{
    public class StationRoutesDTO : IDTO
    {
        public int Id { get; set; } = 0;
        public int StationId { get; set; }
        public int RouteId { get; set; }
    }
}
