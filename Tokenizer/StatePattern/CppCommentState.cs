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
            // peek the first character in the remained line buffer
            int nextItem1 = context_.src.peek();
            if (nextItem1 < 0)
            {
                return false;
            }
            char ch1 = (char)nextItem1;

            if (ch1.Equals('*'))
            {

            }
        }

        override public StringBuilder getTok()
        {
            StringBuilder tok = new StringBuilder();

            return tok;
        }
    }
}
