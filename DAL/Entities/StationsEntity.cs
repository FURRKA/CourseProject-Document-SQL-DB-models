using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Interfaces;

namespace DAL.Entities
{
    public class StationsEntity : IEntity
    {
        public int Id { get; set; }
        public string StationName { get; set; }
    }
}
