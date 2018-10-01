using System;
using System.Text;

namespace Tokenizer
{
    abstract class TokenState : ITokenStateInterface
    {
        internal TokenContext context_ { get; set; }  // derived classes store context ref here

        //----< delegate source opening to context's src >---------------

        public bool open(string path)
        {
            return context_.src.open(path);
        }
        //----< pass interface's requirement onto derived states >-------

        public abstract StringBuilder getTok();

        //----< derived states don't have to know about other states >---

        static public TokenState nextState(TokenContext context)
        {
            int nextItem = context.src.peek();
            if (nextItem < 0)
                return null;
            char ch = (char)nextItem;

            if (Char.IsWhiteSpace(ch))
                return context.ws_;
            if (Char.IsLetterOrDigit(ch))
                return context.as_;

            // Test for strings and comments here since we don't
            // want them classified as punctuators.

            // toker's definition of punctuation is anything that
            // is not whitespace and is not a letter or digit
            // Char.IsPunctuation is not inclusive enough

            return context.ps_;
        }
        //----< has tokenizer reached the end of its source? >-----------

        public bool isDone()
        {
            if (context_.src == null)
                return true;
            return context_.src.end();
        }
    }
}
