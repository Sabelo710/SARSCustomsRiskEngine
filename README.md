SARSCustomsRiskEngine – Quick Docs
What
Tiny library that parses Reverse Polish Notation (RPN) expressions, converts them to infix, and evaluates the result.
Main Types
Table
Copy
Type	Role
Operand	Immutable numeric literal
Operator	Immutable (+ − * /) with precedence & Apply(left,right)
ExpressionNode	Abstract syntax-tree node (ToInfix, Evaluate)
OperandNode	Leaf node holding an Operand
OperatorNode	Internal node with left/right children
RpnParser	Parse(rpnString) → RpnParseResult
RpnParseResult	Returned object; exposes ToInfix() and Evaluate()
Usage
csharp
Copy
var res = new RpnParser().Parse("3 4 + 2 *");
Console.WriteLine(res.ToInfix());   // (3 + 4) * 2
Console.WriteLine(res.Evaluate());  // 14
Errors
Throws RpnParseException for bad tokens or arity.
Extensibility
Add operators or syntax by editing Operator.cs or adding new parsers—no other code changes required.
