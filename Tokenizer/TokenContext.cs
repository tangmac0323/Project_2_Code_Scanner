using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tokenizer
{
    class TokenContext
    {
        internal TokenContext()
        {
            whiteSpaceState_ = new WhiteSpaceState(this);
            puncState_ = new PunctState(this);
            alphaState_ = new AlphaState(this);
            specialPuncState_ = new SpecialPuncState(this);
            dQuoteState_ = new DoubleQuoteState(this);
            sQuoteState_ = new SingleQuoteState(this);
            cCommentState_ = new CCommentState(this);
            cppCommentState_ = new CppCommentState(this);


            // more states here
            currentState_ = whiteSpaceState_;
        }
        internal WhiteSpaceState whiteSpaceState_ { get; set; }
        internal PunctState puncState_ { get; set; }
        internal AlphaState alphaState_ { get; set; }
        // more states here

        internal TokenState currentState_ { get; set; }
        internal ITokenSourceInterface src { get; set; }  // can hold any derived class
    }
}
