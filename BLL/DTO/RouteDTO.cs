using BLL.Interfaces;
using DAL.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace BLL.DTO
{
    public class RouteDTO : IDTO
    {
        [BsonId]
        public int Id { get; set; }
        public string RouteName { get; set; }
        public List<StationDTO> Stations { get; set; } = [];
        public List<DistancesDTO> Distances { get; set; } = [];
        public int GetLenth(int stationId1, int stationId2)
        {
            int stationIndex1 = Stations.IndexOf(Stations.Find(s => s.Id == stationId1));
            int stationIndex2 = Stations.IndexOf(Stations.Find(s => s.Id == stationId2));

            return Distances
                .Skip(Math.Min(stationId1, stationIndex2))
                .Take(Math.Abs(stationIndex1 - stationIndex2))
                .Sum(d => d.Value);
        }
    }
}
