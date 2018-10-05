///////////////////////////////////////////////////////////////////
// CComment class
// - extracts single line comment and tailer comment
using System.Text;


namespace Tokenizer
{
    class CCommentState : TokenState
    {
        public CCommentState(TokenContext context)
        {
            context_ = context;
        }
        //----< manage converting extracted ints to chars >--------------

        bool isCCommentEnd(int i)
        {
            // peek the first character in the remained line buffer
            int nextItem1 = context_.src.peek();
            if (nextItem1 < 0)
            {
                return false;
            }
            char ch1 = (char)nextItem1;

            return ch1.Equals('\n');
        }

        override public StringBuilder getTok()
        {
            StringBuilder tok = new StringBuilder();
            tok.Append((char)context_.src.next()); 

            while (!isCCommentEnd(context_.src.peek()))    // stop when new line
            {
                tok.Append((char)context_.src.next());
            }
            //tok.Append((char)context_.src.next());      // read the new line mark
            return tok;
        }
    }
}

