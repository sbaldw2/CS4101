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
            Token t = scanner.getNextToken();
            while (t != null)
            {
                TokenType tt = t.getType();

                Console.Write(tt);
                if (tt == TokenType.INT)
                    Console.WriteLine(", intVal = " + t.getIntVal());
                else if (tt == TokenType.STRING)
                    Console.WriteLine(", stringVal = " + t.getStringVal());
                else if (tt == TokenType.IDENT)
                    Console.WriteLine(", intVal = " + t.getName());
                else
                    Console.WriteLine();

                //Console.Write("Scheme 4101> ");
                t = scanner.getNextToken();
            }
            return 0;
        }

        //Create Parser
        Parser parser = new Parser(scanner);
        Node root = parser.parseExp();
        while (root != null)
        {
            root.print(0);
            Console.WriteLine("");
            root = parser.parseExp();
        }
        return 0;
    }
}
