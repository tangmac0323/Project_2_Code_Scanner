using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokenizer
{
    class DoubleQuoteState : TokenState
    {
        public DoubleQuoteState(TokenContext context)
        {
            context_ = context;
        }
        //----< manage converting extracted ints to chars >--------------


        bool isPairedDoubleQuote(int i)
        {
            int nextItem = context_.src.peek();
            if (nextItem < 0)
            {
                return false;
            }
            char ch = (char)nextItem;
            return ch.Equals('"');
        }

        override public StringBuilder getTok()
        {
            StringBuilder tok = new StringBuilder();
            tok.Append((char)context_.src.next());
            while (!isPairedDoubleQuote(context_.src.peek()))
            {
                tok.Append((char)context_.src.next());
            }

            tok.Append((char)context_.src.next());  // append the tail quotation
            return tok;
        }
    }
}
