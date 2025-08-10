using System.Globalization;
using SARSCustomsRiskEngine.Abstractions;
using SARSCustomsRiskEngine.Exceptions;
using SARSCustomsRiskEngine.Models;
using SARSCustomsRiskEngine.Nodes;
using SARSCustomsRiskEngine.Results;

namespace SARSCustomsRiskEngine.Parsers
{
    public sealed class RpnParser
    {
        private static readonly HashSet<char> ValidOperators = new() { '+', '-', '*', '/' };

        public RpnParseResult Parse(string rpnExpression)
        {
            if (string.IsNullOrWhiteSpace(rpnExpression))
                throw new RpnParseException("Expression cannot be null or empty");

            var tokens = TokenizeExpression(rpnExpression);
            if (tokens.Count == 0)
                throw new RpnParseException("Expression contains no valid tokens");

            var root = BuildExpressionTree(tokens);
            return new RpnParseResult(root);
        }

        private static List<string> TokenizeExpression(string expression)
        {
            var tokens = new List<string>();
            var current = string.Empty;

            foreach (var c in expression)
            {
                if (char.IsWhiteSpace(c))
                {
                    if (!string.IsNullOrEmpty(current))
                    {
                        tokens.Add(current);
                        current = string.Empty;
                    }
                }
                else if (ValidOperators.Contains(c))
                {
                    if (!string.IsNullOrEmpty(current))
                    {
                        tokens.Add(current);
                        current = string.Empty;
                    }
                    tokens.Add(c.ToString());
                }
                else
                {
                    current += c;
                }
            }

            if (!string.IsNullOrEmpty(current))
                tokens.Add(current);

            return tokens;
        }

        private static ExpressionNode BuildExpressionTree(IReadOnlyList<string> tokens)
        {
            var stack = new Stack<ExpressionNode>();

            for (var i = 0; i < tokens.Count; i++)
            {
                var token = tokens[i];

                if (IsOperator(token))
                {
                    if (stack.Count < 2)
                        throw new RpnParseException($"Insufficient operands for operator '{token}' at position {i}");

                    var right = stack.Pop();
                    var left = stack.Pop();
                    var op = new Operator(token[0]);
                    stack.Push(new OperatorNode(op, left, right));
                }
                else if (IsNumber(token))
                {
                    var operand = new Operand(ParseNumber(token));
                    stack.Push(new OperandNode(operand));
                }
                else
                {
                    throw new RpnParseException($"Invalid token: '{token}'");
                }
            }

            return stack.Count != 1
                ? throw new RpnParseException($"Invalid expression: expected 1 result, got {stack.Count}")
                : stack.Pop();
        }

        private static bool IsOperator(string token) =>
            token.Length == 1 && ValidOperators.Contains(token[0]);

        private static bool IsNumber(string token) =>
            double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out _);

        private static double ParseNumber(string token) =>
            double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out var result)
                ? result
                : throw new RpnParseException($"Invalid number format: '{token}'");
    }
}