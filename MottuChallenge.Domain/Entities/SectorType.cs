using MottuChallenge.Domain.Validations;

namespace MottuChallenge.Domain.Entities
{
    public class SectorType
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public SectorType(string name)
        {
            Guard.AgainstNullOrWhitespace(name, nameof(name), nameof(SectorType));
            this.Id = Guid.NewGuid();
            this.Name = name;
        }

        public void AlterName(string name)
        {
            Guard.AgainstNullOrWhitespace(name, nameof(name), nameof(SectorType));
            this.Name = name;
        }

        private SectorType() { }

    }
}
