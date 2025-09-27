namespace MottuChallenge.Application.DTOs.Response
{
    public class SectorResponseDto
    {
        public Guid Id { get; set; }
        public Guid YardId { get; set; }
        public Guid SectorTypeId { get; set; }
        public List<PointResponseDto> Points { get; set; } = new();
        public List<SpotResponseDto> Spots { get; set; } = new();
    }
}
