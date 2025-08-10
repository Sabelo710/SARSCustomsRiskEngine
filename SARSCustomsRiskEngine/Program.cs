using SARSCustomsRiskEngine.Parsers;

namespace SARSCustomsRiskEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var parser = new RpnParser();

            Console.WriteLine("SARS Customs Risk Engine - RPN Parser");
            Console.WriteLine(new string('=', 40));
            Console.WriteLine("Enter RPN expressions to convert to infix notation and evaluate.");
            Console.WriteLine("Supported operators: +, -, *, /");
            Console.WriteLine("Example: '3 4 +' converts to '3 + 4 = 7'");
            Console.WriteLine("Type 'q' exit.");
            Console.WriteLine();

            while (true)
            {
                Console.Write("Enter RPN expression: ");
                var input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input) ||
                    input.Trim().ToLower() is "q")
                {
                    Console.WriteLine("Goodbye!");
                    break;
                }

                try
                {
                    var result = parser.Parse(input);
                    Console.WriteLine($"Infix Notation:   {result.ToInfix()}");
                    Console.WriteLine($"Result:           {result.Evaluate()}");
                }
                catch (Exception ex) when (
                    ex is Exceptions.RpnParseException ||
                    ex is DivideByZeroException)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
                Console.WriteLine();
            }
        }
    }
}