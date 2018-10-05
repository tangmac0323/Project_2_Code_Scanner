///////////////////////////////////////////////////////////////////
// ITokenSource interface
// - Declares operations expected of any source of tokens
// - Typically we would use either files or strings.  This demo
//   provides a source only for Files, e.g., TokenFileSource, below.

namespace Tokenizer
{
    interface ITokenSourceInterface
    {
        bool open(string path);
        void close();
        int next();
        int peek(int n = 0);
        bool end();
        int lineCount { get; set; }
    }
}
