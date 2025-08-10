using SARSCustomsRiskEngine.Abstractions;
using SARSCustomsRiskEngine.Models;

namespace SARSCustomsRiskEngine.Nodes
{
    public sealed class OperatorNode : ExpressionNode
    {
        private readonly Operator _operator;
        private readonly ExpressionNode _left;
        private readonly ExpressionNode _right;

        public OperatorNode(Operator op, ExpressionNode left, ExpressionNode right)
        {
            _operator = op ?? throw new ArgumentNullException(nameof(op));
            _left = left ?? throw new ArgumentNullException(nameof(left));
            _right = right ?? throw new ArgumentNullException(nameof(right));
        }

        public override string ToInfix()
        {
            var leftStr = _left.ToInfix();
            var rightStr = _right.ToInfix();

            if (_left is OperatorNode leftOp && ShouldAddParentheses(leftOp._operator, _operator, true))
                leftStr = $"({leftStr})";

            if (_right is OperatorNode rightOp && ShouldAddParentheses(rightOp._operator, _operator, false))
                rightStr = $"({rightStr})";

            return $"{leftStr} {_operator} {rightStr}";
        }

        public override double Evaluate()
        {
            var leftValue = _left.Evaluate();
            var rightValue = _right.Evaluate();
            return _operator.Apply(leftValue, rightValue);
        }

        private static bool ShouldAddParentheses(Operator child, Operator parent, bool isLeft)
        {
            if (child.Precedence < parent.Precedence) return true;
            if (child.Precedence > parent.Precedence) return false;

            if (child.Precedence == parent.Precedence)
            {
                if (!isLeft && parent.IsLeftAssociative)
                    return parent.Symbol == '-' || parent.Symbol == '/';
            }
            return false;
        }
    }
}