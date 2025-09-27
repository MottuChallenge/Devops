using MottuChallenge.Domain.Enums;

namespace MottuChallenge.Application.DTOs.Request
{
    public class MotorcycleDto
    {
        public string Model { get; set; }
        public EngineType EngineType { get; set; }
        public string Plate { get; set; }
        public DateTime LastRevisionDate { get; set; }
        public Guid? SpotId { get; set; }
       
    }
}
