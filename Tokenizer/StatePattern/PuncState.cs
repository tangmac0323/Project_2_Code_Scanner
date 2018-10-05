///////////////////////////////////////////////////////////////////
// PunctState class
// - extracts single punctuation char as a token

using System;
using System.Text;


namespace Tokenizer
{
    class PuncState : TokenState
    {
        public PuncState(TokenContext context)
        {
            context_ = context;
        }
        //----< manage converting extracted ints to chars >--------------

        //----< keep extracting until get none-punctuator >--------------

        override public StringBuilder getTok()
        {
            StringBuilder tok = new StringBuilder();
            tok.Append((char)context_.src.next());       // first is punctuator

            return tok;
        }
    }
}
