using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokenizer
{
    class SingleQuoteState : TokenState
    {
        public SingleQuoteState(TokenContext context)
        {
            context_ = context;
        }
        //----< manage converting extracted ints to chars >--------------


        override public StringBuilder getTok()
        {
            StringBuilder tok = new StringBuilder();

            return tok;
        }
    }
}
