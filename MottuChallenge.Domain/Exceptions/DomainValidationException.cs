namespace MottuChallenge.Domain.Exceptions
{
    public class DomainValidationException : Exception
    {
        public string ParameterName { get; }
        public string ClassName { get; }

        public DomainValidationException(string message, string parameterName, string className)
            : base(message)
        {
            ParameterName = parameterName;
            ClassName = className;
        }

        public override string ToString()
        {
            return $"[{ClassName}] Parameter '{ParameterName}': {Message}";
        }
    }
}
