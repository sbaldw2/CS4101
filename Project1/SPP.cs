// SPP -- The main program of the Scheme pretty printer.

using System;
using Parse;
using Tokens;
using Tree;

public class SPP
{
    public static int Main(string[] args)
    {
        // Create scanner that reads from standard input
        Scanner scanner = new Scanner(Console.In);
        
        if (args.Length > 1 ||
            (args.Length == 1 && ! args[0].Equals("-d")))
        {
            Console.Error.WriteLine("Usage: mono SPP [-d]");
            return 1;
        }
        
        // If command line option -d is provided, debug the scanner.
        if (args.Length == 1 && args[0].Equals("-d"))
        {
            // Console.Write("Scheme 4101> ");
            Parser parser = new Parser(scanner);
            Node root;
            Token tok = scanner.getNextToken();
            while (tok != null)
            {
                TokenType tt = tok.getType();
                root = parser.parseExp();
                while (root != null)
                {
                    root.print(0);
                    root = parser.parseExp();
                }
                //Console.Write(tt);
                //if (tt == TokenType.INT)
                //  Console.WriteLine(", intVal = " + tok.getIntVal());
                //else if (tt == TokenType.STRING)
                //  Console.WriteLine(", stringVal = " + tok.getStringVal());
                //else if (tt == TokenType.IDENT)
                //  Console.WriteLine(", name = " + tok.getName());
                //else
                //  Console.WriteLine();

                // Console.Write("Scheme 4101> ");
                tok = scanner.getNextToken();
            }
            return 0;
        }
        return 0;
    }
}
