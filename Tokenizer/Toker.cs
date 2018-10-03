using System.IO;
using System.Collections.Generic;
using System.Text;
using System;

namespace Tokenizer
{
    ///////////////////////////////////////////////////////////////////////
    // class Toker - Tokenizer
    public class Toker
    {
        private TokenContext context_;       // holds single instance of all states and token source
        //private bool keepComment = false;       // switch to keep or discard comment

        //----< initialize state machine >-------------------------------

        public Toker()
        {
            context_ = new TokenContext();      // context is the glue that holds all of the state machine parts 
        }
        //----< attempt to open source of tokens >-----------------------
        /*
         * If src is successfully opened, it uses TokenState.nextState(context_)
         * to set the initial state, based on the source content.
         */
        public bool open(string path)
        {
            TokenSourceFile src = new TokenSourceFile(context_);
            context_.src = src;
            return src.open(path);
        }
        //----< close source of tokens >---------------------------------

        public void close()
        {
            context_.src.close();
        }
        //----< extract a token from source >----------------------------

        private bool isWhiteSpaceToken(StringBuilder tok)
        {
            return (tok.Length > 0 && Char.IsWhiteSpace(tok[0]));
        }

        public StringBuilder getTok()
        {
            StringBuilder tok = null;
            while (!isDone())
            {
                tok = context_.currentState_.getTok();
                context_.currentState_ = TokenState.nextState(context_);
                if (!isWhiteSpaceToken(tok))
                {
                    break;
                }
            }
            return tok;
        }

        /*
        public TokenState getTokState()
        {
            TokenState tokState = null;
            while (!isDone())
            {
                tokState = context_.currentState_;
                context_.currentState_ = TokenState.nextState(context_);

            }
            return tokState;
        }
        */

        //----< has Toker reached end of its source? >-------------------

        public bool isDone()
        {
            if (context_.currentState_ == null)
            {
                return true;
            }
            else
            {
                return false;
            }


            //return context_.currentState_.isDone();
        }


        public int lineCount() { return context_.src.lineCount; }

    }
}
