///////////////////////////////////////////////////////////////////
// SingleQuote class
// - extracts single quoted chars as a token
using System.Text;

namespace Tokenizer
{
    class SingleQuoteState : TokenState
    {
        public SingleQuoteState(TokenContext context)
        {
            context_ = context;
        }
        //----< manage converting extracted ints to chars >--------------
        bool isPairedSingleQuote(int i)
        {
            int nextItem = context_.src.peek();
            if (nextItem < 0)
            {
                return false;
            }
            char ch = (char)nextItem;
            return ch.Equals('\'');
        }

        override public StringBuilder getTok()
        {
            StringBuilder tok = new StringBuilder();

            tok.Append((char)context_.src.next());

            while (!isPairedSingleQuote(context_.src.peek()))
            {
                tok.Append((char)context_.src.next());
            }

            tok.Append((char)context_.src.next());
            return tok;
        }
    }
}
