using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokenizer
{
    class DoubleQuoteState : TokenState
    {
        public DoubleQuoteState(TokenContext context)
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
