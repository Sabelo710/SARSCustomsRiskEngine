namespace SARSCustomsRiskEngine.Abstractions
{
    public abstract class ExpressionNode
    {
        public abstract string ToInfix();
        public abstract double Evaluate();
    }
}