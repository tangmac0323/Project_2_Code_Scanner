using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokenizer
{
    class PuncState : TokenState
    {
        public PuncState(TokenContext context)
        {
            context_ = context;
        }
        //----< manage converting extracted ints to chars >--------------

        bool isPunctuation(int i)
        {
            int nextItem = context_.src.peek();
            if (nextItem < 0)
                return false;
            char ch = (char)nextItem;
            return (!Char.IsWhiteSpace(ch) && !Char.IsLetterOrDigit(ch));
        }
        //----< keep extracting until get none-punctuator >--------------

        override public StringBuilder getTok()
        {
            StringBuilder tok = new StringBuilder();
            tok.Append((char)context_.src.next());       // first is punctuator

            //while (isPunctuation(context_.src.peek()))   // stop when non-punctuator
            //{
                //tok.Append((char)context_.src.next());
            //}
            return tok;
        }
    }
}
