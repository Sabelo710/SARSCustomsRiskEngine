using SARSCustomsRiskEngine.Abstractions;
using SARSCustomsRiskEngine.Models;

namespace SARSCustomsRiskEngine.Nodes
{
    public sealed class OperandNode : ExpressionNode
    {
        private readonly Operand _operand;

        public OperandNode(Operand operand) =>
            _operand = operand ?? throw new ArgumentNullException(nameof(operand));

        public override string ToInfix() => _operand.ToString();
        public override double Evaluate() => _operand.Value;
    }
}