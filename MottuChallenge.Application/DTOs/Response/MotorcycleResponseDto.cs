using MottuChallenge.Domain.Enums;

namespace MottuChallenge.Application.DTOs.Response
{
    public class MotorcycleResponseDto
    {
        public Guid Id { get; set; }
        public string Model { get; set; } = null!;
        public EngineType EngineType { get; set; }
        public string Plate { get; set; } = null!;
        public DateTime LastRevisionDate { get; set; }
        public Guid? SpotId { get; set; }
    }
}
