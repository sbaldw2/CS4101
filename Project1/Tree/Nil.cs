// Nil -- Parse tree node class for representing the empty list

using System;

namespace Tree
{
    public class Nil : Node
    {
        public Nil() { }
  
        public override void print(int n)
        {
            print(n, false);
        }

        public override void print(int n, bool p) {
            var spaces = "";
            if (n >= 0) spaces = new string(' ', n);
            if (p)
                Console.WriteLine(")");
            else
                Console.WriteLine("()");
        }

        public override bool isNull()
        {
            return true;
        }
    }
}
