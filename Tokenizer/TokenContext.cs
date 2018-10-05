///////////////////////////////////////////////////////////////////
// TokenContext class
// - holds all the tokenizer states
// - holds source of tokens
// - internal qualification limits access to this assembly
namespace Tokenizer
{
    class TokenContext
    {
        internal TokenContext()
        {
            whiteSpaceState_ = new WhiteSpaceState(this);
            puncState_ = new PuncState(this);
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
        internal PuncState puncState_ { get; set; }
        internal AlphaState alphaState_ { get; set; }
        internal SpecialPuncState specialPuncState_ { get; set; }
        internal DoubleQuoteState dQuoteState_ { get; set; }
        internal SingleQuoteState sQuoteState_ { get; set; }
        internal CCommentState cCommentState_ { get; set; }
        internal CppCommentState cppCommentState_ { get; set; }

        internal TokenState currentState_ { get; set; }
        internal ITokenSourceInterface src { get; set; }  // can hold any derived class
    }
}
