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

        /// <summary>
        /// Decides whether parentheses are required around a child sub-expression.
        /// Rules:
        /// 1. Lower precedence → always wrap
        /// 2. Higher precedence → never wrap
        /// 3. Same precedence → wrap right operand of left-associative minus or divide
        ///    to keep semantics, e.g. "a - (b - c)" vs "a - b - c".
        /// </summary>
        private static bool ShouldAddParentheses(Operator child, Operator parent, bool isLeft)
        {
            if (child.Precedence < parent.Precedence) return true;
            if (child.Precedence > parent.Precedence) return false;

            if (child.Precedence == parent.Precedence)
            {
                // Same precedence: only right-hand side can need parentheses
                if (!isLeft && parent.IsLeftAssociative)
                    return parent.Symbol == '-' || parent.Symbol == '/';
            }
            return false;
        }
    }
}