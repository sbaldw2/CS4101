using System;
using Tokens;
using Tree;

namespace prog1
{

	// Scanner.java -- the implementation of class Scanner


	internal class Scanner
	{
	 private PushbackInputStream @in;
	 private sbyte[] buf;

	 public Scanner(System.IO.Stream i)
	 {
	  @in = new PushbackInputStream(i);
	 }

	 public virtual Token NextToken
	 {
		 get
		 {
		  int bite = -1;
    
		  // It would be more efficient if we'd maintain our own input buffer
		  // and read characters out of that buffer, but reading individual
		  // characters from the input stream is easier.
		  try
		  {
		   bite = @in.read();
		  }
		  catch (IOException e)
		  {
		   Console.Error.WriteLine("We fail: " + e.Message);
		   // System.err.println("We fail: " + e.getMessage());      // Be sure to uncomment before turning in**************************************************
		  }
    
		  /*
		  if (bite == 10) { // Temporary solution for console input, remove before turning in project****************************************************************
		   try {
		    in.close();
		   } catch (IOException e) {
		    // System.err.println("We fail: " + e.getMessage());
		   }
		  }
		  */
    
		  // TODO: skip white space, carriage return, line feed, and tab 
		  // ****DONE****
    
		  if ((bite == 32) || (bite == 13) || (bite == 10) || (bite == 9))
		  {
		   return NextToken;
		  }
    
		  /*
		  while ((bite == 32) || (bite == 13) || (bite == 10) || (bite == 9)) {
		   try {
		    bite = in.read();
		   } catch (IOException e) {
		    System.err.println("We fail: " + e.getMessage());
		   }
		  }
		  */
    
		  // TODO: skip comments
		  // ****DONE****
		  if (bite == 59)
		  { // 59 is ASCII semicolon
		   while (bite != 10)
		   { // Stop loop at line feed, the end of a comment
			try
			{
			 bite = @in.read();
			}
			catch (IOException e)
			{
			 Console.Error.WriteLine("We fail: " + e.Message);
			}
			if (bite == -1)
			{ // if end of stream is reached at end of a comment, return null
			 return null;
			}
			if (bite == 10)
			{ // arrived to line feed, the end of a comment
			 // At this point, bite contains line feed, so call getNextToken() for the next character of the new line
			 return NextToken;
			} // else, the loop continues
		   }
		  }
    
    
		  if (bite == -1)
		  {
		   return null;
		  }
    
		  char character = (char) bite;
    
		  // Special characters
		  if (character == '\'')
		  {
		   return new Token(Token.QUOTE);
		  }
		  else if (character == '(')
		  {
		   return new Token(Token.LPAREN);
		  }
		  else if (character == ')')
		  {
		   return new Token(Token.RPAREN);
		  }
		  else if (character == '.')
		  {
		   // We ignore the special identifier `...'.
		   return new Token(Token.DOT);
		  }
    
		  // Boolean constants
		  else if (character == '#')
		  {
		   try
		   {
			bite = @in.read();
		   }
		   catch (IOException e)
		   {
			Console.Error.WriteLine("We fail: " + e.Message);
		   }
    
		   if (bite == -1)
		   {
			Console.Error.WriteLine("Unexpected EOF following #");
			return null;
		   }
    
		   character = (char) bite;
		   if (character == 't')
		   {
			return new Token(Token.TRUE);
		   }
		   else if (character == 'f')
		   {
			return new Token(Token.FALSE);
		   }
		   else
		   {
			Console.Error.WriteLine("Illegal character '" + (char) character + "' following #");
			return NextToken;
		   }
		  }
    
		  // String constants
		  else if (character == '"')
		  {
		   buf = new sbyte[1000]; // re-create the buffer to clear pre-existing bytes
		   // TODO: scan a string into the buffer variable buf
		   // ****DONE****
		   try
		   {
			bite = @in.read();
		   }
		   catch (IOException e)
		   {
			Console.Error.WriteLine("We fail: " + e.Message);
		   }
    
		   if (bite == 34)
		   { // check for another double quote here, if it is the case, then we have an empty string, create empty string token
			return new StrToken("");
		   }
    
		   int pointer = 0; // pointer in the buffer array
		   while (bite != 34)
		   { // 34 is ascii for \" (double quote)
			if (bite == -1)
			{
			 Console.Error.WriteLine("Unexpected EOF following \"");
			 return null;
			}
    
			buf[pointer] = (sbyte) bite; // store the bite on the buffer
    
			try
			{ // get the next byte and loop
			 bite = @in.read();
			}
			catch (IOException e)
			{
			 Console.Error.WriteLine("We fail: " + e.Message);
			}
    
			pointer = pointer + 1;
		   }
    
		   return new StrToken(StringHelperClass.NewString(buf));
		   // old code: return new StrToken(buf.toString());
		   // new String(buf) converts all ascii code in array to string
		  }
    
		  // Integer constants
		  else if (character >= '0' && character <= '9')
		  {
		   // TODO: scan the number and convert it to an integer
		   // ****DONE****
    
		   string number = ""; // Hacky way to read a number, but works
    
		   while (bite >= 48 && bite <= 57)
		   {
			number += character;
    
			try
			{ // get the next byte
			 bite = @in.read();
			}
			catch (IOException e)
			{
			 Console.Error.WriteLine("We fail: " + e.Message);
			}
    
			if (bite == -1)
			{
			 Console.Error.WriteLine("Unexpected EOF following an Integer");
			 return null;
			}
    
			character = (char) bite;
		   }
    
		   // put the character after the integer back into the input
		   // in->putback(ch);
		   try
		   {
			@in.unread(bite);
		   }
		   catch (IOException e)
		   {
			Console.Error.WriteLine("We fail: " + e.Message);
		   }
    
		   int i = int.Parse(number);
    
		   return new IntToken(i);
		  }
    
		  // Identifiers
		  else if ((character >= 'A' && character <= 'Z') || (character >= 'a' && character <= 'z') || (character == '!') || (character == '$') || (character == '%') || (character == '&') || (character == '*') || (character == '/') || (character == ':') || (character == '<') || (character == '=') || (character == '>') || (character == '?') || (character == '^') || (character == '_') || (character == '~') || (character == '+') || (character == '-'))
			// Special Initials
			// Peculiar Identifier
		  { // Initial letters, both upper and lower cases
		   // TODO: scan an identifier into the buffer
		   // ****DONE****
    
		   // Need to check and return a Peculiar Identifier as token before rest of the grammar happens
		   if ((character == '+') || (character == '-'))
		   {
			return new IdentToken("" + character);
		   }
    
		   buf = new sbyte[1000]; // re-create the buffer to clear pre-existing bytes
    
		   buf[0] = (sbyte) character;
    
		   try
		   { // get the next byte
			bite = @in.read();
		   }
		   catch (IOException e)
		   {
			Console.Error.WriteLine("We fail: " + e.Message);
		   }
    
		   if (bite == -1)
		   {
			Console.Error.WriteLine("Unexpected EOF following an Identifier");
			// return null;
		   }
    
		   character = (char) bite;
    
		   int pointer = 1;
		   while ((character >= 'A' && character <= 'Z') || (character >= 'a' && character <= 'z') || (character == '!') || (character == '$') || (character == '%') || (character == '&') || (character == '*') || (character == '/') || (character == ':') || (character == '<') || (character == '=') || (character == '>') || (character == '?') || (character == '^') || (character == '_') || (character == '~') || (character >= '0' && character <= '9') || (character == '+') || (character == '-') || (character == '.') || (character == '@'))
			// Special Initials
			// Digit
			// Special subsequent
		   { // Initial letters, both upper and lower cases
			buf[pointer] = (sbyte) character;
    
			try
			{ // get the next byte
			 bite = @in.read();
			}
			catch (IOException e)
			{
			 Console.Error.WriteLine("We fail: " + e.Message);
			}
    
			if (bite == -1)
			{
			 Console.Error.WriteLine("Unexpected EOF following an Identifier");
			 // return null; 
			}
    
			character = (char) bite;
			pointer = pointer + 1;
		   }
		   // put the character after the identifier back into the input
		   // in->putback(ch);
		   try
		   {
			@in.unread(bite);
		   }
		   catch (IOException e)
		   {
			Console.Error.WriteLine("We fail: " + e.Message);
		   }
    
		   return new IdentToken(StringHelperClass.NewString(buf));
		  }
    
		  // Illegal character
		  else
		  {
		   Console.Error.WriteLine("Illegal input character '" + (char) character + '\'');
		   return NextToken;
		  }
		 }
	 }
	}

}