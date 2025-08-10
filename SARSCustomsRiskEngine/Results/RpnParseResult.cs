using SARSCustomsRiskEngine.Abstractions;

namespace SARSCustomsRiskEngine.Results
{
    public sealed class RpnParseResult
    {
        private readonly ExpressionNode _root;

        public RpnParseResult(ExpressionNode root) =>
            _root = root ?? throw new ArgumentNullException(nameof(root));

        public string ToInfix() => _root.ToInfix();
        public double Evaluate() => _root.Evaluate();
    }
}