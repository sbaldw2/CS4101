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
            return parseExp(scanner.getNextToken());
        }

        public Node parseExp(Token currentToken) // lookahead
        {
            var t = currentToken;
            if (t == null)
            {
                return null;
            }
            else if (t.getType() == TokenType.LPAREN)
            {
                return parseRest();
            }
            else if (t.getType() == TokenType.TRUE)
            {
                return new BoolLit(true);
            }
            else if (t.getType() == TokenType.FALSE)
            {
                return new BoolLit(false);
            }
            else if (t.getType() == TokenType.QUOTE)
            {
                return new Cons(new Ident("quote"), new Cons(parseExp(), new Nil()));
            }
            else if (t.getType() == TokenType.INT)
            {
                return new IntLit(t.getIntVal());
            }
            else if (t.getType() == TokenType.STRING)
            {
                return new StringLit(t.getStringVal());
            }
            else if (t.getType() == TokenType.IDENT)
            {
                return new Ident(t.getName());
            }

            return null;
        }

        protected Node parseRest() // no lookahead
        {
            return parseRest(scanner.getNextToken());
        }

        public Node parseRest(Token currentToken) // lookahead
        {
            var t = currentToken;
            if (t == null)
            {
                return null;
            }
            else if (t.getType() == TokenType.RPAREN)
            {
                return new Nil();
            }
            else
            {
                return new Cons(parseExp(t), parserRest());

            }
            // TODO: dot expressions, leave this for now
            //Console.Error.WriteLine("end of if/elses");
            //Nil error = new Nil();
            //error.print(1);
            //return null;
        }

        protected Node parserRest()
        {
            var t = scanner.getNextToken();
            if (t == null)
            {
                return null;
            }
            else if (t.getType() == TokenType.DOT)
            {
                var temp = parseExp();
                t = scanner.getNextToken();
                if (t.getType() == TokenType.RPAREN)
                {
                    return temp;
                }

                return null;
            }
            else
            {
                return parseRest(t);
            }
        }

        // TODO: Add any additional methods you might need.
    }
}
