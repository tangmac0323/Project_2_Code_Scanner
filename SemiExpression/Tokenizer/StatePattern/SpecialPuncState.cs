using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
