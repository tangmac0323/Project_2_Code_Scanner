///////////////////////////////////////////////////////////////////
// WhiteSpaceState class
// - extracts contiguous whitespace chars as a token
// - will be thrown away by tokenizer

using System;
using System.Text;

namespace Tokenizer
{
    class WhiteSpaceState : TokenState
    {
        public WhiteSpaceState(TokenContext context)
        {
            context_ = context;
        }
        //----< manage converting extracted ints to chars >--------------

        bool isWhiteSpace(int i)
        {
            int nextItem = context_.src.peek();
            if (nextItem < 0)
                return false;
            char ch = (char)nextItem;
            return Char.IsWhiteSpace(ch);
        }
        //----< keep extracting until get none-whitespace >--------------

        override public StringBuilder getTok()
        {
            StringBuilder tok = new StringBuilder();
            tok.Append((char)context_.src.next());     // first is WhiteSpace

            while (isWhiteSpace(context_.src.peek()))  // stop when non-WhiteSpace
            {
                tok.Append((char)context_.src.next());
            }
            return tok;
        }
    }
}
