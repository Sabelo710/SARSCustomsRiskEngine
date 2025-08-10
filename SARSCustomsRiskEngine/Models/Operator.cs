namespace SARSCustomsRiskEngine.Models
{
    public sealed class Operator
    {
        public char Symbol { get; }
        public int Precedence { get; }
        public bool IsLeftAssociative { get; }

        public Operator(char symbol)
        {
            Symbol = symbol;
            (Precedence, IsLeftAssociative) = GetOperatorProperties(symbol);
        }

        private static (int precedence, bool isLeftAssociative) GetOperatorProperties(char op) =>
            op switch
            {
                '+' or '-' => (1, true),
                '*' or '/' => (2, true),
                _ => throw new ArgumentException($"Unsupported operator: {op}", nameof(op))
            };

        /// <summary>
        /// Applies the operator to two operands.
        /// Guards against divide-by-zero and unknown symbols.
        /// </summary>
        public double Apply(double left, double right) =>
            Symbol switch
            {
                '+' => left + right,
                '-' => left - right,
                '*' => left * right,
                // Explicit check to give a domain-specific exception instead of Infinity/NaN
                '/' => right != 0 ? left / right : throw new DivideByZeroException("Division by zero"),
                _ => throw new InvalidOperationException($"Unknown operator: {Symbol}")
            };

        public override string ToString() => Symbol.ToString();
    }
}