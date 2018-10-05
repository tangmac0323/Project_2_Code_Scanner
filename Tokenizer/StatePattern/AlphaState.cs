///////////////////////////////////////////////////////////////////
// AlphaState class
// - extracts contiguous letter and digit chars as a token

using System;
using System.Text;

namespace Tokenizer
{
    class AlphaState : TokenState
    {
        public AlphaState(TokenContext context)
        {
            context_ = context;
        }
        //----< manage converting extracted ints to chars >--------------

        bool isLetterOrDigit(int i)
        {
            int nextItem = context_.src.peek();
            if (nextItem < 0)
                return false;
            char ch = (char)nextItem;
            return Char.IsLetterOrDigit(ch);
        }
        //----< keep extracting until get none-alpha >-------------------

        override public StringBuilder getTok()
        {
            StringBuilder tok = new StringBuilder();
            tok.Append((char)context_.src.next());          // first is alpha

            while (isLetterOrDigit(context_.src.peek()))    // stop when non-alpha
            {
                tok.Append((char)context_.src.next());
            }
            return tok;
        }
    }
}
