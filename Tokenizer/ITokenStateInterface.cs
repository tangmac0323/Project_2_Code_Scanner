///////////////////////////////////////////////////////////////////
// ITokenState interface
// - Declares operations expected of any token gathering state

using System.Text;

namespace Tokenizer
{
    interface ITokenStateInterface
    {
        StringBuilder getTok();
        bool isDone();
    }
}
