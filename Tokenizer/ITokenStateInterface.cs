using System.Text;


namespace Tokenizer
{
    interface ITokenStateInterface
    {
        StringBuilder getTok();
        bool isDone();
    }
}
