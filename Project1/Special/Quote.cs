// Quote -- Parse tree node strategy for printing the special form quote

using System;

namespace Tree
{
    public class Quote : Special
    {
        // TODO: Add any fields needed.
  
        // TODO: Add an appropriate constructor.
	public Quote() { }

        public override void print(Node a, int b, bool c)
        {
            var spaces = new string(' ', b);
            Console.Write(spaces + "\'");
            a = a.getCdr();
            do
            {
                if (a.getCar().isPair())
                {
                    (a.getCar() as Cons).form = new Regular();
                }
                a.getCar().print(0);
                a = a.getCdr();
            }

            while (a.getCdr() != null);
        }
    }
}

