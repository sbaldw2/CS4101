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
                Nil error = new Nil();
                error.print(1);
                return null;
            }
            else if (currentToken.getType() == TokenType.LPAREN)
            {
                Token peekToken = scanner.getNextToken();
                if (peekToken == null)
                {
                    Console.Error.WriteLine("Syntax Error - Unexpected EOF inside expression. Repearing error and terminating Parser.");
                    Nil error = new Nil();
                    error.print(1);
                    return null;
                }
                else if (peekToken.getType() == TokenType.RPAREN)
                {
                    Nil rightParen = new Nil();
                    rightParen.print(1);
                    return null;
                }
                else
                {
                    return new Cons(parseExp(peekToken), parseRest());
                }

            }
            else if (currentToken.getType() == TokenType.TRUE)
            {
                // return new Boollit(true).print(1);
                BoolLit trueToken = new BoolLit(true);
                trueToken.print(1);
                return trueToken;
            }
            else if (currentToken.getType() == TokenType.FALSE)
            {
                BoolLit falseToken = new BoolLit(false);
                falseToken.print(1);
                return falseToken;
            }
            else if (currentToken.getType() == TokenType.QUOTE)
            {
                Node quoteNode = parseExp();
                if (quoteNode.isNull())
                {
                    return new Cons(new Ident("quote"), new Nil());
                }
                return new Cons(new Ident("quote"), new Cons(quoteNode, new Nil())); // something with trees idk
            }
            else if (currentToken.getType() == TokenType.INT)
            {
                IntLit intToken = new IntLit(currentToken.getIntVal());
                return intToken;// have no idea how to do this
            }
            else if (currentToken.getType() == TokenType.STRING)
            {
                StringLit stringToken = new StringLit(currentToken.getStringVal()); // this either
                return stringToken;
            }
            else if (currentToken.getType() == TokenType.IDENT)
            {
                return new Ident(currentToken.getName());
            }
            else
            {
                Console.Error.WriteLine("Syntax Error - Illegal parse token type, deleting token from stream.");
                 // Node deleteNode = parseExp(deleteNode); something with trees idk
                // if (deleteNode == null) tree shit
                //{
                    //return new Nil();
               // }
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
                return new Nil();
            }
            else if (currentToken.getType() == TokenType.LPAREN)
            {
                return new Cons(parseExp(currentToken), parseRest());
            }
            else if (currentToken.getType() == TokenType.RPAREN)
            {
                return new Nil();
            }
            else if (currentToken.getType() == TokenType.QUOTE)
            {
                return new Cons(parseExp(currentToken), parseRest());
            }
            else
            {
                Token peekToken = scanner.getNextToken();
                if (peekToken == null)
                {
                    Console.Error.WriteLine("Syntax Error - Unexpected EOF inside expression. Repearing error and terminating Parser.");
                    Nil errorNode = new Nil();
                    errorNode.print(1);
                    return null;
                }
            }
            // TODO: dot expressions, leave this for now
            Console.Error.WriteLine("end of if/elses");
            Nil error = new Nil();
            error.print(1);
            return null;
        }

        // TODO: Add any additional methods you might need.
    }
}
