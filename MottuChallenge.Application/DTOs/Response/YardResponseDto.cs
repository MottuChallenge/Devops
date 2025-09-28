namespace MottuChallenge.Application.DTOs.Response
{
    public class YardResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public AddressResponseDto Address { get; set; }
        public List<PointResponseDto> Points { get; set; } = new();
    }
}
