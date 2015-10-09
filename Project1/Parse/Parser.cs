// Parser -- the parser for the Scheme printer and interpreter
//
// Defines
//
//   class Parser;
//
// Parses the language
//
//   exp  ->  ( rest
//         |  #f
//         |  #t
//         |  ' exp
//         |  integer_constant
//         |  string_constant
//         |  identifier
//    rest -> )
//         |  exp+ [. exp] )
//
// and builds a parse tree.  Lists of the form (rest) are further
// `parsed' into regular lists and special forms in the constructor
// for the parse tree node class Cons.  See Cons.parseList() for
// more information.
//
// The parser is implemented as an LL(0) recursive descent parser.
// I.e., parseExp() expects that the first token of an exp has not
// been read yet.  If parseRest() reads the first token of an exp
// before calling parseExp(), that token must be put back so that
// it can be reread by parseExp() or an alternative version of
// parseExp() must be called.
//
// If EOF is reached (i.e., if the scanner returns a NULL) token,
// the parser returns a NULL tree.  In case of a parse error, the
// parser discards the offending token (which probably was a DOT
// or an RPAREN) and attempts to continue parsing with the next token.

using System;
using Tokens;
using Tree;

namespace Parse
{
    public class Parser
    {

        private Scanner scanner;

        public Parser(Scanner s)
        {
            scanner = s;
        }

        public Node parseExp() // no lookahead
        {
            Token currentToken = scanner.getNextToken();
            if (currentToken == null)
            {
                Console.Error.WriteLine("Unexpected EOF following breaking parser");
                return null;
            }
            return parseExp(currentToken);
        }

        public Node parseExp(Token currentToken) // no lookahead
        {
            currentToken = scanner.getNextToken();
            if (currentToken == null)
            {
                Console.Error.WriteLine("Unexpected EOF following breaking parser");
                return null;
                // TODO, add null token??
            }
            return null;
        }

        protected Node parseRest() // no lookahead
        {
            Token currentToken = scanner.getNextToken();
            if (currentToken == null)
            {
                Console.Error.WriteLine("Unexpected EOF following breaking parser");
                return null;
            }
            return parseExp(currentToken);
        }

        public Node parseRest(Token currentToken) // no lookahead
        {
            currentToken = scanner.getNextToken();
            if (currentToken == null)
            {
                Console.Error.WriteLine("Unexpected EOF following breaking parser");
                return null;
                // TODO, add null token??
            }
            return null;
        }

        // TODO: Add any additional methods you might need.
    }
}