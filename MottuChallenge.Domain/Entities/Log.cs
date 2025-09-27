using MottuChallenge.Domain.Exceptions;
using MottuChallenge.Domain.Validations;

namespace MottuChallenge.Domain.Entities
{
    public class Log
    {
        public Guid Id { get; private set; }
        public string Message { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public Guid MotorcycleId { get; private set; }
        public Motorcycle Motorcycle { get; private set; }
        public Spot PreviousSpot { get; private set; }
        public Guid PreviousSpotId { get; private set; }
        public Spot DestinationSpot { get; private set; }
        public Guid DestinationSpotId { get; private set; }

        public Log(string message, Guid motorcycleId, Guid previousSpotId, Guid destinationSpotId)
        {
            ValidateMessage(message);
            this.Id = Guid.NewGuid();
            this.Message = message;
            this.CreatedAt = DateTime.UtcNow;
            this.MotorcycleId = motorcycleId;
            this.PreviousSpotId = previousSpotId;
            this.DestinationSpotId = destinationSpotId;
        }

        private Log() { }

        private void ValidateMessage(string message)
        {
            Guard.AgainstNullOrWhitespace(message, nameof(message), nameof(Log));
            if (message.Length > 150)
            {
                throw new DomainValidationException("Message must have less than 150 characters", nameof(message), nameof(Log));
            }
        }
    }
}
