// Scanner -- The lexical analyzer for the Scheme printer and interpreter

using System;
using System.IO;
using Tokens;

namespace Parse
{
    public class Scanner
    {
        private TextReader In;

        // maximum length of strings and identifier
        private const int BUFSIZE = 1000;
        private char[] buf = new char[BUFSIZE];

        public Scanner(TextReader i) { In = i; }
  
        // TODO: Add any other methods you need

        public Token getNextToken()
        {
            int ch;

            try
            {
                // It would be more efficient if we'd maintain our own
                // input buffer and read characters out of that
                // buffer, but reading individual characters from the
                // input stream is easier. peek when you do ints/strs. add white space check. 
                ch = In.Read(); // sees next char
                if (ch == 10 || ch == 32 || ch == 9 || ch == 13 ) //isEmpty()?
                    return getNextToken(); // yeah dog keep going
                else if (ch == 59) // hit a ;
                {
                     while(ch != 10) // as long don't hit end of line
                     {
                        ch = In.Read(); // next char
                        if (ch == -1 || ch == 0) // if stream breaks
                        {
                            Console.Error.WriteLine("Illegal input character '" + (char)ch + '\'');
                            break; // return error (null)
                        }
                     }
                     if (ch == 10) // at da end
                        return getNextToken();
                }
   
                // TODO: skip white space and comments
                // use ascii characters to check 

                else if (ch == -1 || ch == 0)
                    return null;
        
                // Special characters
                else if (ch == '\'')
                    return new Token(TokenType.QUOTE);
                else if (ch == '(')
                    return new Token(TokenType.LPAREN);
                else if (ch == ')')
                    return new Token(TokenType.RPAREN);
                else if (ch == '.')
                    // We ignore the special identifier `...'.
                    return new Token(TokenType.DOT);
                
                // Boolean constants
                else if (ch == '#')
                {
                    ch = In.Read();
                    if (ch == 't')
                        return new Token(TokenType.TRUE);
                    else if (ch == 'f')
                        return new Token(TokenType.FALSE);
                    else if (ch == -1)
                    {
                        Console.Error.WriteLine("Unexpected EOF following #");
                        return null;
                    }
                    else
                    {
                        Console.Error.WriteLine("Illegal character '" +
                                                (char)ch + "' following #");
                        return getNextToken();
                    }
                }

                // String constants
                else if (ch == '"') //it's a string!
                {
                    buf = new buf[BUFSIZE]; // clean out buffer for new str
                    if (ch == 34) // if empty string
                        return new StringToken(new String(""));
                    int StrCounter = 0; // counter/position for buffer
                    while (ch != 34) 
                    {
                        if (ch = -1)
                        {
                            Console.Error.WriteLine("Illegal character '" + (char)ch + "' following #");
                            return getNextToken();
                        }
                        buf[StrCounter] = (byte) ch; // store current byte value
                        ch = In.Next(); // peek to check next byte
                        if (ch = -1) // err check
                        {
                            Console.Error.WriteLine("Illegal character '" + (char)ch + "' following #");
                            return getNextToken();
                        }
                        StrCounter = StrCounter + 1; // increment counter
                    }
                    return new StringToken(new String(buf, 0, StrCounter));
                }

    
                // Integer constants--check if this is read as ASCII or int or what?
                // majorly confusing method. Look up how to do this??? Or ask Sam.
                else if (ch >= '0' && ch <= '9')
                {
                    while (ch >= 48 && ch <= 57)
                    {
                        int i = ch - '0'; // this converts to int
                        ch = In.Next();
                        if(ch == -1)
                        {
                            Console.Error.WriteLine("Illegal character '" + (char)ch + "' following #");
                            return getNextToken();
                        }
                        return new IntToken(i);
                    }
                }
        
                // Identifiers
                else if (ch >= 'A' && ch <= 'Z' || (ch >= 'a' && ch <= 'z')
                         // or ch is some other valid first character
                         // for an identifier
                         ) {
                    // TODO: scan an identifier into the buffer

                    // make sure that the character following the integer
                    // is not removed from the input stream

                    return new IdentToken(new String(buf, 0, 0));
                }
    
                // Illegal character
                else
                {
                    Console.Error.WriteLine("Illegal input character '"
                                            + (char)ch + '\'');
                    return getNextToken();
                }
            }
            catch (IOException e)
            {
                Console.Error.WriteLine("IOException: " + e.Message);
                return null;
            }
        }

        //Comment out or delete main function upon submission
        //Only for testing the scanner
        static void main(String[] args)
        {
            Console.WriteLine(""+ch+"");
        }
    }

}

