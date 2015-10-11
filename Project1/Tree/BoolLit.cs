// BoolLit -- Parse tree node class for representing boolean literals

using System;

namespace Tree
{
    public class BoolLit : Node
    {
        private bool boolVal;
  
        public BoolLit(bool b)
        {
            boolVal = b;
        }
  
        public override void print(int n)
        {
            var spaces = "";
            if (n >= 0)
                spaces = new string(' ', n);

            if (boolVal)
                Console.WriteLine("#t");
            else
                Console.WriteLine("#f");
        }

        public override bool isBool()
        {
            return true;
        }
    }
}
