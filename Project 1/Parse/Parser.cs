using System;
using Tokens;
using Tree;

namespace prog1
{

	// Parser.java -- the implementation of class Parser
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
	//  
	//    modification for the grammar above:
	//
	//    rest -> )
	//          | exp rest
	//          | exp . exp )   // Only one expression can fit between DOT and RPAREN
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

	internal class Parser
	{
	 private Scanner scanner;

	 public Parser(Scanner s)
	 {
	  scanner = s;
	 }

	 public virtual Node parseExp()
	 { // This is the method to use for non-lookahead tokens
	  // TODO: write code for parsing an exp
	  Token tok = scanner.NextToken;
	  if (tok == null)
	  {
	   return null;
	  }
	  return parseExp(tok);
	 }

	 private Node parseExp(Token tok)
	 { // This is the method to use for lookahead tokens
	  if (tok == null)
	  {
	   Console.Error.WriteLine("Syntax Error - Unexpected EOF inside expression. Repearing error and terminating Parser.");
	   return Nil.Instance;
	  }
	  else if (tok.Type == TokenType.LPAREN)
	  {
	   Token lookahead_tok = scanner.NextToken; // We want to check for RPAREN if it follows immediately after LPAREN
	   if (lookahead_tok == null)
	   {
		Console.Error.WriteLine("Syntax Error - Unexpected EOF inside expression. Repearing error and terminating Parser.");
		return Nil.Instance;
	   }
	   else if (lookahead_tok.Type == TokenType.RPAREN)
	   {
		return Nil.Instance; // If so, return NIL
	   }
	   else
	   {
		return new Cons(parseExp(lookahead_tok), parseRest());
	   }
	  }
	  else if (tok.Type == TokenType.FALSE)
	  {
	   return BooleanLit.FalseBoolean;
	  }
	  else if (tok.Type == TokenType.TRUE)
	  {
	   return BooleanLit.TrueBoolean;
	  }
	  else if (tok.Type == TokenType.QUOTE)
	  {
	   Node node = parseExp();
	   if (node.Null)
	   {
		return new Cons(new Ident("quote"), Nil.Instance);
	   }
	   return new Cons(new Ident("quote"), new Cons(node, Nil.Instance));
	  }
	  else if (tok.Type == TokenType.INT)
	  {
	   return new IntLit(tok.IntVal);
	  }
	  else if (tok.Type == TokenType.STRING)
	  {
	   return new StrLit(tok.StrVal);
	  }
	  else if (tok.Type == TokenType.IDENT)
	  {
	   return new Ident(tok.Name);
	  }
	  else
	  {
	   Console.Error.WriteLine("Syntax Error - Illegal Expression parse token type: '" + tok.Type + '\'' + "\nDiscarding Illegal token and attempting to parse further.");
	   Node node = parseExp();
	   if (node == null)
	   {
		node = Nil.Instance;
	   }
	   return node;
	  }
	 }

	 protected internal virtual Node parseRest()
	 {
	  // TODO: write code for parsing rest
	  Token tok = scanner.NextToken;
	  if (tok == null)
	  {
	   Console.Error.WriteLine("Syntax Error - Unexpected EOF inside expression. Repearing error and terminating Parser.");
	   return Nil.Instance;
	  }
	  return parseRest(tok);
	 }

	 private Node parseRest(Token tok)
	 { // This is the method for lookahead token inside parseRest
	  if (tok == null)
	  {
	   Console.Error.WriteLine("Syntax Error - Unexpected EOF inside expression. Repearing error and terminating Parser.");
	   return Nil.Instance;
	  }
	  else if (tok.Type == TokenType.LPAREN)
	  {
	   return new Cons(parseExp(tok),parseRest());
	  }
	  else if (tok.Type == TokenType.RPAREN)
	  {
	   return Nil.Instance;
	  }
	  else if (tok.Type == TokenType.QUOTE)
	  { // Check for nested quotes
	   return new Cons(parseExp(tok), parseRest());
	  }
	  else
	  {
	   Token lookahead_tok = scanner.NextToken;

	   // Code might look weird here because the feature to check dots was added after the parser became fully functional parsing non-dotted expressions.
	   // So I didn't really want to edit pre-existing code in case I break something.

	   if (lookahead_tok == null)
	   {
		Console.Error.WriteLine("Syntax Error - Unexpected EOF inside expression. Repearing error and terminating Parser.");
		return Nil.Instance;
	   }

	   // Check for DOT notion
	   if (tok.Type == TokenType.DOT)
	   { // There is a DOT notion!
		// Need to prevent another DOT or RPAREN being immediately after dot
		while ((lookahead_tok.Type == TokenType.DOT) || (lookahead_tok.Type == TokenType.RPAREN))
		{
		 Console.Error.WriteLine("Syntax Error - Illegal Expression after DOT, Token Type: " + lookahead_tok.Type + ". Discarding offending expression...");
		 lookahead_tok = scanner.NextToken;
		 if (lookahead_tok == null)
		 {
		  Console.Error.WriteLine("Syntax Error - EOF reached inside DOT expression. Repearing error and terminating Parser.");
		  return Nil.Instance;
		 }
		}

		// At this point, the first expression after DOT has been found, store this as tok
		tok = lookahead_tok;

		// we want to parse this token, in case it is an expression inside a set of parenthesis
		Node node = parseExp(tok);

		// Need to make sure there's only one expression between the DOT and RPAREN
		int paren_counter = 1; // Counts the number of left parenthesis found while removing extra expressions

		while (!(paren_counter <= 0))
		{ // have the while loop stop when paren_counter = 0
		 lookahead_tok = scanner.NextToken;

		 if (lookahead_tok == null)
		 {
		  Console.Error.WriteLine("Syntax Error - Missing Right Parenthesis after DOT (EOF reached.) Ending parse at last valid experession.");
		  paren_counter = 0;
		 }
		 else if (lookahead_tok.Type == TokenType.RPAREN)
		 {
		  paren_counter = paren_counter - 1;
		 }
		 else if (lookahead_tok.Type == TokenType.LPAREN)
		 {
		  paren_counter = paren_counter + 1;
		 }
		 else
		 {
		  Console.Error.WriteLine("Syntax Error - Extra Expression after DOT. Discarding offending expression...");
		 }
		}
		return node;
	   }
	   else
	   { // There's no DOT notion
		if (lookahead_tok.Type == TokenType.QUOTE)
		{ // Check for nested quotes
		 return new Cons(parseExp(tok), new Cons(parseExp(lookahead_tok), parseRest()));
		}
		else
		{
		 return new Cons(parseExp(tok), parseRest(lookahead_tok));
		}
	   }
	  }
	 }
	 // TODO: Add any additional methods you might need.
	}

}