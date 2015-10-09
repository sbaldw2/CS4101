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
                if (isEmpty(ch)) //isEmpty()?
                    return getNextToken(); // yeah dog keep going
                else if (ch == 59) // hit a ;--but see if we can condense into isEmpty??
                {
                     while(ch != 10) // as long don't hit end of line
                     {
                        ch = In.Read(); // next char
                        if (ch == -1 ) // if stream breaks
                        {
                            return null;
                        }
                     }
                     if (ch == 10) // at da end
                        return getNextToken();
                }
   
                // TODO: skip white space and comments
                // use ascii characters to check

                else if (ch == -1)
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

                // String constants--TURN INTO LOWERCASE
                else if (ch == '"') //it's a string!
                {
                    buf = new buf[BUFSIZE]; // clean out buffer for new str
                    if (ch == '"') // if empty string
                        return new StringToken(new String(''));
                    int StrCounter = 0; // counter/position for buffer
                    while (ch != '"') 
                    {
                        if (ch = -1)
                        {
                            return null;
                        }
                        buf[StrCounter] = (char) ch; // store current byte value
                        ch = In.Next(); // check next byte
                        if (ch = -1) // err check
                        {
                            return null;
                        }
                        StrCounter = StrCounter + 1; // increment counter
                    }
                    String tempString = new String(buf,0,StrCounter); // prep for lowercase
                    tempString = tempString.ToLower(); // lowercase
                    return new StringToken(tempString);
                }

    
                // Integer constants--check if this is read as ASCII or int or what?
                // majorly confusing method. Look up how to do this? do we still need this resolved?
                else if (isNum(ch)) // see helper method
                {
                    while (isNum(ch)) // verify 
                    {
                        buf = new buf[BUFSIZE]; // read in the chars
                        int intCounter = 0;
                        ch = In.Next();
                        if(ch == -1)
                        {
                            return null;
                        }
                        buf[intCounter] = (char) ch;
                    }
                    int i = Int32.Parse(buf);
                    return new IntToken(i);
                }
        
                // Identifiers
                else if (isIdentValid(ch))
                {
                    int identCounter = 0; // start the position 
                    buf = new buf[BUFSIZE]; // clean buffer
                    while (isIdentValid(ch))
                    {
                        char previousIdentifier = (char)ch;
                        identCounter++;
                        buf[i] = (char) ch;
                        ch = In.Next();
                    }
                    String tempString = new String(buf,0,StrCounter); // prep for lowercase
                    tempString = tempString.ToLower(); // lowercase
                    return new IdentToken(tempString);
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

        private bool isEmpty(int ch)
        {
            return (ch == 10 || ch == 32 || ch == 9 || ch == 13 );
        }

        private bool isNum(int ch)
        {
            return (ch >= '0' && ch <= '9');
        }

        private bool isIdentValid(int ch)
        {
            return (ch >= 'A' && ch <= 'Z')
                || (ch >= 'a' && ch <= 'z')
                // nums
                || (ch >= '0' && ch <= '9')
                // non-numbers and letters
                || (ch == '!')
                || (ch == '$')
                || (ch == '%')
                || (ch == '&')
                || (ch == '*')
                || (ch == '/')
                || (ch == ':')
                || (ch == '<')
                || (ch == '=')
                || (ch == '>')
                || (ch == '?')
                || (ch == '^')
                || (ch == '_')
                || (ch == '~')
                // special identifiers
                || (ch == '+')
                || (ch == '-');
        }
    }

}