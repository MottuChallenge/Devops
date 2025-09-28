namespace MottuChallenge.Application.DTOs.Request
{
    public class CreateYardDto
    {
        public string Name { get; set; } = null!;
        public string Cep { get; set; } = null!;
        public string Number { get; set; } = null!;
        public List<CreatePolygonPointDto> Points { get; set; } = new();
    }
}
