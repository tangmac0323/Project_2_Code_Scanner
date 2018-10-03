using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokenizer
{
    class CppCommentState : TokenState
    {
        public CppCommentState(TokenContext context)
        {
            context_ = context;
        }
        //----< manage converting extracted ints to chars >--------------
        bool isCppCommentEnd(int i)
        {
            // peek the first character 
            int nextItem1 = context_.src.peek();
            if (nextItem1 < 0)
            {
                return false;
            }
            char ch1 = (char)nextItem1;

            // check tail mark
            if (ch1.Equals('*'))
            {
                // check if the next is '/'
                // peek the second character 
                int nextItem2 = context_.src.peek(1);
                if (nextItem2 < 0)
                {
                    return true;
                }
                char ch2 = (char)nextItem2;

                if (ch2.Equals('/'))
                {
                    return true;
                }

            }

            return false;
        }

        override public StringBuilder getTok()
        {
            StringBuilder tok = new StringBuilder();
            tok.Append((char)context_.src.next());          // first is alpha

            while (!isCppCommentEnd(context_.src.peek()))    // stop when non-alpha
            {
                tok.Append((char)context_.src.next());
            }

            tok.Append((char)context_.src.next());  // read in the last element
            tok.Append((char)context_.src.next());  // read in the last element
            return tok;
        }
    }
}
