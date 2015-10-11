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
                return null;
            }
            return parseExp(currentToken);
        }

        public Node parseExp(Token currentToken) // lookahead
        {
            currentToken = scanner.getNextToken();
            if (currentToken == null)
            {
                Console.Error.WriteLine("Syntax Error - Unexpected EOF inside expression. Repearing error and terminating Parser.");
                return new Nil();
            }
            else if (currentToken.getType() == TokenType.LPAREN)
            {
                Token peekToken = scanner.getNextToken();
                if (peekToken == null)
                {
                    Console.Error.WriteLine("Syntax Error - Unexpected EOF inside expression. Repearing error and terminating Parser.");
                    return new Nil();
                }
                else if (peekToken.getType() == TokenType.RPAREN)
                {
                    return new Nil();
                }
                else
                {
                    return new Cons(parseExp(peekToken), parseRest());
                }

            }
            else if (currentToken.getType() == TokenType.TRUE)
            {
                return new BoolLit(true);
            }
            else if (currentToken.getType() == TokenType.FALSE)
            {
                return new BoolLit(false);
            }
            return null;
        }

        protected Node parseRest() // no lookahead
        {
            Token currentToken = scanner.getNextToken();
            if (currentToken == null)
            {
                Console.Error.WriteLine("Syntax Error - Unexpected EOF inside expression. Repearing error and terminating Parser.");
                return null;
            }
            return parseExp(currentToken);
        }

        public Node parseRest(Token currentToken) // lookahead
        {
            currentToken = scanner.getNextToken();
            if (currentToken == null)
            {
                Console.Error.WriteLine("Syntax Error - Unexpected EOF inside expression. Repearing error and terminating Parser.");
                return null;
                // TODO, add null token??
            }
            return null;
        }

        // TODO: Add any additional methods you might need.
    }
}
