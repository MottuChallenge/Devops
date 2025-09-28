
namespace MottuChallenge.Application.DTOs.Request
{
    public class SectorCreateDto
    {
        public Guid YardId { get; set; } 
        public Guid SectorTypeId { get; set; }         
        public List<CreatePolygonPointDto> Points { get; set; } = new();
    }
}
