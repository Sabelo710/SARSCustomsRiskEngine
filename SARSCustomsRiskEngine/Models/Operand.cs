using System.Globalization;

namespace SARSCustomsRiskEngine.Models
{
    public sealed class Operand
    {
        public double Value { get; }

        public Operand(double value) => Value = value;

        public override string ToString() =>
            Value % 1 == 0
                ? Value.ToString("F0", CultureInfo.InvariantCulture)
                : Value.ToString(CultureInfo.InvariantCulture);
    }
}