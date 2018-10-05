///////////////////////////////////////////////////////////////////
// SpecialPunctState class
// - extracts double punctuation char as a token

using System.Text;

namespace Tokenizer
{
    class SpecialPuncState : TokenState
    {
        public SpecialPuncState(TokenContext context)
        {
            context_ = context;
        }
        //----< manage converting extracted ints to chars >--------------

        override public StringBuilder getTok()
        {
            StringBuilder tok = new StringBuilder();

            tok.Append((char)context_.src.next());
            tok.Append((char)context_.src.next());
            return tok;
        }


    }
}
