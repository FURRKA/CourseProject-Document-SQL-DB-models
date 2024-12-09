using BLL.Interfaces;

namespace BLL.DTO
{
    public class TicketOrdersDTO : IDTO
    {
        public int Id { get; set; } = 0;
        public int TicketId { get; set; }
        public int OrderId { get; set; }
    }
}
