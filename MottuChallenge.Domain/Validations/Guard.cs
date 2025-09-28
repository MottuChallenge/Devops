using MottuChallenge.Domain.Exceptions;

namespace MottuChallenge.Domain.Validations
{
    public static class Guard
    {
        public static void AgainstNullOrEmpty(Guid id, string name, string className)
        {
            if (id == Guid.Empty)
                throw new DomainValidationException($"{name} must not be empty", name, className);
        }

        public static void AgainstNullOrWhitespace(string value, string name, string className)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainValidationException($"{name} must not be null or empty", name, className);
        }

        public static void AgainstNegativeCoordinates(double x, double y, string className, string nameX = "x", string nameY = "y")
        {
            if (x < 0)
                throw new DomainValidationException($"{nameX} coordinate must be non-negative", nameX, className);

            if (y < 0)
                throw new DomainValidationException($"{nameY} coordinate must be non-negative", nameY, className);
        }

    }
}
