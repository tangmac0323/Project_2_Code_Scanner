using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
