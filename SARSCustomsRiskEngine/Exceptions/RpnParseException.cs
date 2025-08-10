namespace SARSCustomsRiskEngine.Exceptions
{
    public sealed class RpnParseException : Exception
    {
        public RpnParseException(string message) : base(message) { }
        public RpnParseException(string message, Exception inner) : base(message, inner) { }
    }
}